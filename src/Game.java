package javecs;


import java.util.HashMap;

public class Game {
    private boolean isRunning;

    public void initialize() {
        GameObject.instantiate("Player");
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
