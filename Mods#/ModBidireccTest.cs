using MSharp.ModAPI;
using System;
using MSharp.Launcher.Core.Bridge;

namespace ModDeEjemplo
{
	public class ModVioleta : IMsharpMod
	{
		private readonly IBridgeConnection _bridge;

		public ModVioleta(IBridgeConnection bridge) => this._bridge = bridge;

		public void OnStart() =>  Console.WriteLine("🟪 Mod Violeta iniciado.");
		

		public void OnEvent(string type, object? payload = null)
		{
			Console.WriteLine($"📨 Evento recibido desde Java: {type} | Payload: {payload}");

			if (type == "BRIDGE_MSG")  _bridge.Send($"ACK: {payload}");
			
		}
		public void OnTick() { }

	}
}
