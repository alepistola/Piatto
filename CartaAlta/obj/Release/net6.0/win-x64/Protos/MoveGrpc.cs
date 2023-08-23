// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/move.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace CartaAlta.Grpc {
  public static partial class MoveService
  {
    static readonly string __ServiceName = "move.MoveService";

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
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.MovePost> __Marshaller_move_MovePost = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.MovePost.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.MoveStatus> __Marshaller_move_MoveStatus = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.MoveStatus.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.RequestNumber> __Marshaller_move_RequestNumber = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.RequestNumber.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::CartaAlta.Grpc.Move> __Marshaller_move_Move = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::CartaAlta.Grpc.Move.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::CartaAlta.Grpc.MovePost, global::CartaAlta.Grpc.MoveStatus> __Method_BroadcastMove = new grpc::Method<global::CartaAlta.Grpc.MovePost, global::CartaAlta.Grpc.MoveStatus>(
        grpc::MethodType.Unary,
        __ServiceName,
        "BroadcastMove",
        __Marshaller_move_MovePost,
        __Marshaller_move_MoveStatus);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::CartaAlta.Grpc.RequestNumber, global::CartaAlta.Grpc.Move> __Method_GetByNumber = new grpc::Method<global::CartaAlta.Grpc.RequestNumber, global::CartaAlta.Grpc.Move>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetByNumber",
        __Marshaller_move_RequestNumber,
        __Marshaller_move_Move);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::CartaAlta.Grpc.MoveReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of MoveService</summary>
    [grpc::BindServiceMethod(typeof(MoveService), "BindService")]
    public abstract partial class MoveServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::CartaAlta.Grpc.MoveStatus> BroadcastMove(global::CartaAlta.Grpc.MovePost request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::CartaAlta.Grpc.Move> GetByNumber(global::CartaAlta.Grpc.RequestNumber request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for MoveService</summary>
    public partial class MoveServiceClient : grpc::ClientBase<MoveServiceClient>
    {
      /// <summary>Creates a new client for MoveService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public MoveServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for MoveService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public MoveServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected MoveServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected MoveServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.MoveStatus BroadcastMove(global::CartaAlta.Grpc.MovePost request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return BroadcastMove(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.MoveStatus BroadcastMove(global::CartaAlta.Grpc.MovePost request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_BroadcastMove, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.MoveStatus> BroadcastMoveAsync(global::CartaAlta.Grpc.MovePost request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return BroadcastMoveAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.MoveStatus> BroadcastMoveAsync(global::CartaAlta.Grpc.MovePost request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_BroadcastMove, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.Move GetByNumber(global::CartaAlta.Grpc.RequestNumber request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetByNumber(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::CartaAlta.Grpc.Move GetByNumber(global::CartaAlta.Grpc.RequestNumber request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetByNumber, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.Move> GetByNumberAsync(global::CartaAlta.Grpc.RequestNumber request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetByNumberAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::CartaAlta.Grpc.Move> GetByNumberAsync(global::CartaAlta.Grpc.RequestNumber request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetByNumber, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override MoveServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new MoveServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(MoveServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_BroadcastMove, serviceImpl.BroadcastMove)
          .AddMethod(__Method_GetByNumber, serviceImpl.GetByNumber).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, MoveServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_BroadcastMove, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CartaAlta.Grpc.MovePost, global::CartaAlta.Grpc.MoveStatus>(serviceImpl.BroadcastMove));
      serviceBinder.AddMethod(__Method_GetByNumber, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::CartaAlta.Grpc.RequestNumber, global::CartaAlta.Grpc.Move>(serviceImpl.GetByNumber));
    }

  }
}
#endregion
