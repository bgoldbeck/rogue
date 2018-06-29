package ecs;

import java.util.HashMap;
import java.util.ArrayList;
import components.*;

public class GameObject {
    private static HashMap<String, GameObject> gameObjects = new HashMap<>();

    private ArrayList<Component> components;
    private boolean isActive;
    private String tag;
    public Position position;

    public GameObject() {
        this.tag = "";
        this.isActive = true;
        this.components = new ArrayList<>();
    }

    public String getTag() {
        return tag;
    }

    public void start() {
        for (Component component: components) {
            if (component.isActive()) {
                component.start();
            }
        }
        return;
    }

    public void update() {

        for (Component component: components) {
            if (component.isActive()) {
                component.update();
            }
        }
        return;
    }

    public void render() {
        for (Component component: components) {
            if (component.isActive()) {
                component.render();
            }
        }
        return;
    }

    public void addComponent(Component component) {
        if (getComponent(component.getClass()) == null) {
            // We point the component's game object to point to this game object.
            component.gameObject = this;
            this.components.add(component);

        }
        return;
    }

    public Component getComponent(Class<?> type) {
        Component retrieved = null;

        for (Component component: components) {
            if (component.getClass() == type) {
                retrieved = component;
                break;
            }
        }

        return retrieved;
    }

    public void removeComponent(Class<?> type) {
        components.remove(type);
        return;
    }

    public static GameObject instantiate(String tag) {
        // Game object tags must be unique.
        if (gameObjects.containsKey(tag)) {
            return null;
        }

        GameObject go = new GameObject();
        // Every game object will have a position component.
        Position position = new Position();
        go.addComponent(position);
        go.position = position;

        // Add the game object to the data structure.
        gameObjects.put(tag, go);

        return go;
    }

    public static void destroy(String tag) {
        gameObjects.remove(tag);
        return;
    }

    public static GameObject find(String tag) {
        return gameObjects.get(tag);
    }

    public static HashMap<String, GameObject> getGameObjects() {
        return gameObjects;
    }
}
