package com.example.msharpmod;
import net.minecraftforge.fml.common.Mod;
import net.minecraftforge.fml.common.event.FMLInitializationEvent;

@Mod(modid = "msharp", version = "0.0.1", name = "MSharp Mod test")
public class MSharpMod {

    @Mod.EventHandler
    public void init(FMLInitializationEvent event) {

        System.out.println("[MSharpMod] Mod recibido, inicializando...");

        InstructionRegister.register("game", new GenericGameActionHandler());

        InstructionRegister.register("textura", new TextureHandler());
    }
}
