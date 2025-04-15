package com.msharp.bridgemod;

import net.minecraftforge.fml.common.Mod;
import net.minecraftforge.fml.common.event.FMLInitializationEvent;

import java.io.FileOutputStream;
import java.io.IOException;
import java.util.concurrent.Executors;

@Mod(modid = "msharpbridgefinal777", name = "MSharp Bridge", version = "1.3", acceptedMinecraftVersions = "[1.8.9]")
public class MSharpBridgeMod {

    private static final String PIPE_NAME = "\\\\.\\pipe\\msharp_bridge";

    @Mod.EventHandler
    public void init(FMLInitializationEvent event) {
        log("✅ MSharp Bridge (pipe) iniciado.");

        Executors.newSingleThreadExecutor().submit(() -> {
            int reintentos = 5;

            while (reintentos > 0) {
                try (FileOutputStream pipe = new FileOutputStream(PIPE_NAME)) {
                    String msg = "ON_START";
                    pipe.write(msg.getBytes());
                    pipe.flush();
                    log("📤 Mensaje enviado al pipe: " + msg);
                    break;
                } catch (IOException e) {
                    reintentos--;
                    log("⚠️  Error escribiendo al pipe (reintentos restantes: " + reintentos + "): " + e.getMessage());
                    try {
                        Thread.sleep(1000);
                    } catch (InterruptedException ignored) {}
                }
            }

            if (reintentos == 0) {
                log("❌ No se pudo establecer conexión con el pipe luego de varios intentos.");
            }
        });
    }

    private void log(String msg) {
        System.out.println("[MSharpBridge] " + msg);
    }
}
