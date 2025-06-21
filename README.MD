# MSharpCore ModLoader: Modding C# para Minecraft


## ¿Qué es M#?

**SharpCore ModLoader** es un framework que permite crear mods para Minecraft (1.8.9 Forge, por ahora) usando **C#** y herramientas modernas como Visual Studio, integrando la lógica de los mods con el juego en tiempo real a través de un puente entre **Java** y **C#**.  
El objetivo es **democratizar el modding, hacerlo más accesible y moderno**.

---

## Características principales

- **Mods en C#**: Escribe mods usando C# y Visual Studio, sin depender de Java.
- **Comunicación en tiempo real**: Sincronización bidireccional entre Minecraft (Java) y mods en C# usando Named Pipes.
- **Carga dinámica de mods**: Los mods se cargan **como archivos .m#** y pueden reaccionar a eventos del juego.
- **Arquitectura extensible**: Pensado para integrar herramientas visuales como Unity en el futuro.
- **Ejemplo de mod**: Incluye un mod de ejemplo para empezar rápido.

---

## Arquitectura

```
+-------------------+         Named Pipe         +-------------------+
|   Minecraft (Java)| <-----------------------> |    C# Launcher    |
|  + Forge Mod      |                           | + ModLoader       |
|  | MSharpBridge   |                           | + Mods (IMsharpMod)|
+-------------------+                           +-------------------+
```

- **Java Side:** Mod Forge (`MSharpBridgeMod`) que actúa como cliente de la pipe.
- **C# Side:** Launcher que carga mods, gestiona la pipe y despacha eventos.
- **Comunicación:** Named Pipes para mensajes en tiempo real.

---

## ¿Cómo funciona?

1. **Lanzador en C#**: Inicia Minecraft y el servidor de Named Pipe.
2. **Carga de mods**: Busca y carga DLLs de mods escritos en C#.
3. **Mod puente en Java**: Se conecta al pipe y envía/recibe mensajes con el launcher.
4. **Eventos y lógica**: Los mods en C# reaccionan a eventos del juego y pueden enviar comandos de vuelta a Minecraft.



-

## Ejemplo de mod en C#

```csharp
public class ModVioleta : IMsharpMod
{
    public void OnStart() => Console.WriteLine("🟪 Mod Violeta iniciado.");
    public void OnEvent(string type, object? payload = null)
    {
        Console.WriteLine($"📨 Evento recibido desde Java: {type} | Payload: {payload}");
    }
    public void OnTick() { }
}
```

## Licencia

Este proyecto se distribuye bajo la licencia LGPL v2.1 o posterior (ver archivos LICENSE).

