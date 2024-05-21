# Piatto - Gioco di Carte Peer-to-Peer

**Candidato:** Alessandro Pistola  
**Data:** 27 Settembre 2023  
**Università di Bologna - Distributed Software Systems**

## Indice

1. [Piatto: Gioco e Regole](#piatto-gioco-e-regole)
2. [Caratteristiche](#caratteristiche)
3. [Servizi](#servizi)
4. [P2P Server - Client](#p2p-server-client)
5. [Sincronizzazione](#sincronizzazione)
6. [Flusso Matchmaking](#flusso-matchmaking)
7. [Flusso di Gioco](#flusso-di-gioco)
8. [Gestione Disconnessione](#gestione-disconnessione)
9. [Gestione Crash](#gestione-crash)

## Piatto: Gioco e Regole

### Piatto: Il Gioco

Piatto è un gioco d’azzardo della cultura Jesina (AN) il cui nome deriva dal piatto dove vengono messe le scommesse. Si gioca con un mazzo da 40 carte regionali ed il numero di giocatori è illimitato, sebbene un numero maggiore di 15-20 giocatori potrebbe inficiare l’esperienza di gioco.

### Piatto: Le Regole

- All’inizio e quando il piatto si svuota, ogni giocatore deve versare nel piatto la puntata minima.
- Il dealer viene scelto casualmente e ruota ogni 2 turni completi.
- Ad ogni turno di un giocatore questo deve scegliere la sua puntata (>= puntata minima) e svelare la prima carta del mazzo (< 6 perde, >= 6 vince, asso e re fanno rispettivamente perdere e vincere il doppio).
- Lo scopo del gioco è quello di rimanere l’unico giocatore al tavolo con un bilancio maggiore di zero.

## Caratteristiche

### Middleware gRPC

gRPC è una tecnologia di comunicazione tra processi che consente di connettere, invocare ed eseguire applicazioni distribuite con la stessa facilità come se si stesse lavorando in un ambiente non distribuito.


### gRPC + .NET 6.0

gRPC si integra bene con l’ecosistema C# esistente, rendendolo facile da incorporare nelle applicazioni peer-to-peer. Ad esempio, è possibile utilizzare gRPC con ASP.NET Core per creare applicazioni server.

![gRPC.NET](https://github.com/alepistola/Piatto/blob/master/img/grpc-logo.png)


### Topologia Rete e State Sharing

La topologia di rete peer to peer scelta è quella strutturata ad anello. Per sviluppare un’applicazione con una tolleranza completa agli arresti anomali si è optato per il meccanismo di sincronizzazione dello stato (o state sharing) "Serverless Data Blast".

![Ring topology](https://github.com/alepistola/Piatto/blob/master/img/Ring-topolog.png)


## Servizi

### Servizi gRPC - Lobby

I servizi sono stati divisi in 3 file: game.proto, lobby.proto e move.proto.

- **lobby.proto**
  ```proto
  service Matchmaking {
      rpc RegisterForMatch (Peer) returns (RegisterReply);
      rpc StartMatch (StartMatchInfo) returns (StartMatchAck);
  }
  ```

### Servizi gRPC - GameService

- **game.proto**
  ```proto
  service GameService {
      rpc PassTurn(PassTurnRequest) returns (GameServiceResponse);
      rpc SyncDeck(DeckState) returns (GameServiceResponse);
      rpc AskInitialBet(InitialBetRequest) returns (GameServiceResponse);
      rpc EndGame(EndGameRequest) returns (GameServiceResponse);
      rpc SignalCrash(CrashInfo) returns (GameServiceResponse);
      rpc Ping(GameServiceRequest) returns (GameServiceResponse);
  }
  ```

### Servizi gRPC - Move

- **move.proto**
  ```proto
  service MoveService {
      rpc BroadcastMove(MovePost) returns (MoveStatus);
  }
  ```

## P2P Server - Client

### P2P Server

Implementare un server P2P con gRPC è abbastanza semplice, basta estendere i servizi definiti nei file .proto e avviare ogni servizio all’interno di `startup.cs`, mappando le richieste al tipo specifico.

![P2P Server](https://github.com/alepistola/Piatto/blob/master/img/ping.PNG)

### P2P Service mapping

![P2P service mapping](https://github.com/alepistola/Piatto/blob/master/img/mapser.PNG)

### P2P Client

Creare un client P2P è altrettanto semplice, prima si crea un canale con l’URL del peer come parametro, poi si chiama il servizio e si richiama la funzione.

![P2P broadcast move](https://github.com/alepistola/Piatto/blob/master/img/broadmo.png)


## Sincronizzazione

La sincronizzazione dello stato avviene in istanti diversi:

- **Mazzo**: avviene attraverso la ricezione di una rpc `SyncDeck(DeckState)` oppure attraverso l’inizializzazione del mazzo (se si è dealer).
- **Giocatori, bilanci e mosse**: ogni giocatore che modifica lo stato locale propaga le modifiche agli altri che di conseguenza verificano la mossa (legit check) e si aggiornano di conseguenza.

## Aggiornamento stato

![P2P update](https://github.com/alepistola/Piatto/blob/master/img/update.PNG)

## Flusso Matchmaking

![Matchmaking Activity Diagram](https://github.com/alepistola/Piatto/blob/master/img/matchmakingActivity.png)

## Flusso di Gioco

### Game Flow Activity Diagram

Punto di vista del singolo peer.

![Game Flow Activity Diagram](https://github.com/alepistola/Piatto/blob/master/img/gameUML.png)

### Game Flow Sequence Diagram

Estratto di una comunicazione, prima e dopo aver compiuto una mossa.

![Game Flow Sequence Diagram](https://github.com/alepistola/Piatto/blob/master/img/CroppedDealerTurn.png)

## Gestione Disconnessione

### Concetti

Due concetti simili:

- **Disconnessione**: la volontaria uscita dal gioco.
- **Crash**: arresto in modo anomalo, ovvero inaspettato.

Per entrambi si sono gestiti i casi in cui ad uscire dal gioco fosse il giocatore nel pieno del proprio turno di gioco o un giocatore in attesa.

### Disconnessione

Essendo una scelta prevista è presente una rpc per gestire tale eventualità `EndGame(EndGameRequest)`. Si procede quindi con le operazioni di safe shutdown e rpc `EndGame`. Qualora a disconnettersi sia il giocatore che deve effettuare il turno di gioco, le operazioni sono le medesime con l’aggiunta di una `PassTurnRequest`, infatti se così non fosse, nessun giocatore potrebbe compiere la mossa successiva.

### Disconnection Sequence Diagram

![Disconnection Sequence Diagram](https://github.com/alepistola/Piatto/blob/master/img/Disconnection.png)

![Disconnection Sequence Diagram Turn](https://github.com/alepistola/Piatto/blob/master/img/DisconnectionTurn.png)

## Gestione Crash

### Gestione Crash

Come nel caso precedente si differenziano le due casistiche.

Quando il peer che si arresta non è colui che deve effettuare una mossa di gioco, il rilevamento dell’arresto avviene “passivamente” attraverso la routine di gestione dell’errore di rete chiamata `HandlePeerCrash(Peer)` che in ordine:
1. Rimuove il peer dal database dei peer.
2. Aggiorna il peer adiacente all’interno del P2PService.
3. Rimuove il giocatore associato al peer dall’elenco dei giocatori del GameEngine.
4. Fa uso della rpc `SignalCrash` per segnalare il crash agli altri peer.

### Crash Activity Diagram

![Crash Activity Diagram](https://github.com/alepistola/Piatto/blob/master/img/HandlePeerCrash.png)

### Crash Detection Service

Quando ad arrestarsi in modo anomalo è il peer che deve compiere la mossa di gioco, il meccanismo messo in atto finora non è efficace. Crash di questa tipologia sono rilevabili solo da un servizio sviluppato appositamente, il crash detection service.

- Utilizzo di un Timer.
- Restart del medesimo ad ogni ricezione di un aggiornamento di stato di gioco.
- Sfruttamento del meccanismo di rilevazione “passiva” (`HandlePeerCrash`).
- Gestione del crash del dealer.
- Gestione crash multipli.

### CrashDetectionService Activity Diagram

![CrashDetectionService Activity Diagram](https://github.com/alepistola/Piatto/blob/master/img/CrashDetectionService.png)

