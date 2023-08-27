// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/game.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace CartaAlta.Grpc {
  public static partial class GameService
  {
    static readonly string __ServiceName = "game.GameService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.PassTurnRequest> __Marshaller_game_PassTurnRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.PassTurnRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.GameServiceResponse> __Marshaller_game_GameServiceResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.GameServiceResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.DeckState> __Marshaller_game_DeckState = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.DeckState.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.InitialBetRequest> __Marshaller_game_InitialBetRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.InitialBetRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.EndGameRequest> __Marshaller_game_EndGameRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.EndGameRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.CrashInfo> __Marshaller_game_CrashInfo = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.CrashInfo.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::CartaAlta.Grpc.PassTurnRequest, global::CartaAlta.Grpc.GameServiceResponse> __Method_PassTurn = new grpc::Method<global::CartaAlta.Grpc.PassTurnRequest, global::CartaAlta.Grpc.GameServiceResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "PassTurn",
        __Marshaller_game_PassTurnRequest,
        __Marshaller_game_GameServiceResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::CartaAlta.Grpc.DeckState, global::CartaAlta.Grpc.GameServiceResponse> __Method_SyncDeck = new grpc::Method<global::CartaAlta.Grpc.DeckState, global::CartaAlta.Grpc.GameServiceResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SyncDeck",
        __Marshaller_game_DeckState,
        __Marshaller_game_GameServiceResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::CartaAlta.Grpc.InitialBetRequest, global::CartaAlta.Grpc.GameServiceResponse> __Method_AskInitialBet = new grpc::Method<global::CartaAlta.Grpc.InitialBetRequest, global::CartaAlta.Grpc.GameServiceResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "AskInitialBet",
        __Marshaller_game_InitialBetRequest,
        __Marshaller_game_GameServiceResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::CartaAlta.Grpc.EndGameRequest, global::CartaAlta.Grpc.GameServiceResponse> __Method_EndGame = new grpc::Method<global::CartaAlta.Grpc.EndGameRequest, global::CartaAlta.Grpc.GameServiceResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "EndGame",
        __Marshaller_game_EndGameRequest,
        __Marshaller_game_GameServiceResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::CartaAlta.Grpc.CrashInfo, global::CartaAlta.Grpc.GameServiceResponse> __Method_SignalCrash = new grpc::Method<global::CartaAlta.Grpc.CrashInfo, global::CartaAlta.Grpc.GameServiceResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SignalCrash",
        __Marshaller_game_CrashInfo,
        __Marshaller_game_GameServiceResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::CartaAlta.Grpc.GameReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of GameService</summary>
    [grpc::BindServiceMethod(typeof(GameService), "BindService")]
    public abstract partial class GameServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::CartaAlta.Grpc.GameServiceResponse> PassTurn(global::CartaAlta.Grpc.PassTurnRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::CartaAlta.Grpc.GameServiceResponse> SyncDeck(global::CartaAlta.Grpc.DeckState request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::CartaAlta.Grpc.GameServiceResponse> AskInitialBet(global::CartaAlta.Grpc.InitialBetRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::CartaAlta.Grpc.GameServiceResponse> EndGame(global::CartaAlta.Grpc.EndGameRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::CartaAlta.Grpc.GameServiceResponse> SignalCrash(global::CartaAlta.Grpc.CrashInfo request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for GameService</summary>
    public partial class GameServiceClient : grpc::ClientBase<GameServiceClient>
    {
      /// <summary>Creates a new client for GameService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public GameServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for GameService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public GameServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected GameServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected GameServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse PassTurn(global::CartaAlta.Grpc.PassTurnRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PassTurn(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse PassTurn(global::CartaAlta.Grpc.PassTurnRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_PassTurn, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> PassTurnAsync(global::CartaAlta.Grpc.PassTurnRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return PassTurnAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> PassTurnAsync(global::CartaAlta.Grpc.PassTurnRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_PassTurn, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse SyncDeck(global::CartaAlta.Grpc.DeckState request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SyncDeck(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse SyncDeck(global::CartaAlta.Grpc.DeckState request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SyncDeck, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> SyncDeckAsync(global::CartaAlta.Grpc.DeckState request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SyncDeckAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> SyncDeckAsync(global::CartaAlta.Grpc.DeckState request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SyncDeck, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse AskInitialBet(global::CartaAlta.Grpc.InitialBetRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AskInitialBet(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse AskInitialBet(global::CartaAlta.Grpc.InitialBetRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_AskInitialBet, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> AskInitialBetAsync(global::CartaAlta.Grpc.InitialBetRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AskInitialBetAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> AskInitialBetAsync(global::CartaAlta.Grpc.InitialBetRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_AskInitialBet, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse EndGame(global::CartaAlta.Grpc.EndGameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EndGame(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse EndGame(global::CartaAlta.Grpc.EndGameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_EndGame, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> EndGameAsync(global::CartaAlta.Grpc.EndGameRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return EndGameAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> EndGameAsync(global::CartaAlta.Grpc.EndGameRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_EndGame, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse SignalCrash(global::CartaAlta.Grpc.CrashInfo request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SignalCrash(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.GameServiceResponse SignalCrash(global::CartaAlta.Grpc.CrashInfo request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SignalCrash, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> SignalCrashAsync(global::CartaAlta.Grpc.CrashInfo request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SignalCrashAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.GameServiceResponse> SignalCrashAsync(global::CartaAlta.Grpc.CrashInfo request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SignalCrash, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override GameServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new GameServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(GameServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_PassTurn, serviceImpl.PassTurn)
          .AddMethod(__Method_SyncDeck, serviceImpl.SyncDeck)
          .AddMethod(__Method_AskInitialBet, serviceImpl.AskInitialBet)
          .AddMethod(__Method_EndGame, serviceImpl.EndGame)
          .AddMethod(__Method_SignalCrash, serviceImpl.SignalCrash).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, GameServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_PassTurn, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CartaAlta.Grpc.PassTurnRequest, global::CartaAlta.Grpc.GameServiceResponse>(serviceImpl.PassTurn));
      serviceBinder.AddMethod(__Method_SyncDeck, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CartaAlta.Grpc.DeckState, global::CartaAlta.Grpc.GameServiceResponse>(serviceImpl.SyncDeck));
      serviceBinder.AddMethod(__Method_AskInitialBet, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CartaAlta.Grpc.InitialBetRequest, global::CartaAlta.Grpc.GameServiceResponse>(serviceImpl.AskInitialBet));
      serviceBinder.AddMethod(__Method_EndGame, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CartaAlta.Grpc.EndGameRequest, global::CartaAlta.Grpc.GameServiceResponse>(serviceImpl.EndGame));
      serviceBinder.AddMethod(__Method_SignalCrash, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CartaAlta.Grpc.CrashInfo, global::CartaAlta.Grpc.GameServiceResponse>(serviceImpl.SignalCrash));
    }

  }
}
#endregion
