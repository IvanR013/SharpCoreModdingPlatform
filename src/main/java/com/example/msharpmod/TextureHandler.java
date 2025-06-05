package com.example.msharpmod;

import org.json.JSONObject;

public class TextureHandler implements InstructionHandler {
    @Override
    public void handle(JSONObject instruction) {
        String entidad = instruction.optString("entidad");
        String archivo = instruction.optString("archivo");
        System.out.println("[MSharpMod] Aplicar textura '" + archivo + "' a entidad '" + entidad + "'");
    }
}

