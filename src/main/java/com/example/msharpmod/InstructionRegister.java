package com.example.msharpmod;

import org.json.JSONObject;
import java.util.HashMap;
import java.util.Map;

public class InstructionRegister {
	private static final Map<String, InstructionHandler> handlers = new HashMap<>();
	
	public static void register(String tipo, InstructionHandler handler) {
		
		handlers.put(tipo, handler);
		
	}
	
	public static void dispatch(JSONObject instruction) {
		
		String tipo = instruction.optString("tipo");
		
		InstructionHandler handler = handlers.get(tipo);
		
		if (handler != null) {
			handler.handle(instruction);
		}
		else {
			System.out.println("[MSharpMod] Handler no encontrado para tipo: " + tipo);
		}
	}
}





