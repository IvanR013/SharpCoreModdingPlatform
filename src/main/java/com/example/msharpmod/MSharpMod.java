package com.example.msharpmod;
import net.minecraftforge.fml.common.Mod;
import net.minecraftforge.fml.common.event.FMLInitializationEvent;

@Mod(modid = "msharp", version = "1.0", name = "MSharp Mod")
public class MSharpMod {

    @Mod.EventHandler
    public void init(FMLInitializationEvent event) {

        System.out.println("[MSharpMod] Inicializando mod...");

        InstructionRegister.register("game", new GenericGameActionHandler());

        InstructionRegister.register("textura", new TextureHandler());
    }
}
