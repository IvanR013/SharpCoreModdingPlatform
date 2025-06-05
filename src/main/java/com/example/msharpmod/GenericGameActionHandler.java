package com.example.msharpmod;
import net.minecraft.client.Minecraft;
import net.minecraft.entity.player.EntityPlayer;
import net.minecraft.init.Items;
import net.minecraft.item.ItemStack;
import org.json.JSONObject;

public class GenericGameActionHandler implements InstructionHandler {

    @Override
    public void handle(JSONObject instruction) {
        String accion = instruction.optString("accion");
        String item = instruction.optString("item");
        int cantidad = instruction.optInt("cantidad", 1);

        EntityPlayer player = Minecraft.getMinecraft().player;
        if (player == null) {
            System.out.println("[MSharpMod] El jugador no está disponible.");
            return;
        }

        if ("give".equalsIgnoreCase(accion)) {
            if ("diamond".equalsIgnoreCase(item)) {
                player.inventory.addItemStackToInventory(new ItemStack(Items.DIAMOND, cantidad));
                System.out.println("[MSharpMod] Dando " + cantidad + " diamantes al jugador.");
            } else {
                System.out.println("[MSharpMod] Item desconocido: " + item);
            }
        } else {
            System.out.println("[MSharpMod] Acción no soportada: " + accion);
        }
    }
}
