import ecs.*;
import components.*;
import java.util.HashMap;


public class Game {
    private boolean isRunning = true;

    public void initialize() {
        GameObject player = GameObject.instantiate("Player");
        player.addComponent(new Player());
        Transform transform = (Transform)player.getComponent(Transform.class);

        System.out.println("Player x:" + transform.position.x + " Player y:" + transform.position.y);
        System.out.println("Player parent: " + transform.parent);
        return;
    }

    public void loop() {
        while(isRunning) {
            update();
            render();
            isRunning = false;
        }
    }

    public void update() {

        HashMap<String, GameObject> map = GameObject.getGameObjects();
        for (String key : map.keySet()) {
            map.get(key).update();
        }
    }

    public void render() {
        HashMap<String, GameObject> map = GameObject.getGameObjects();
        for (String key : map.keySet()) {
            map.get(key).render();
        }
    }



}
