package ecs;

public class Component {
    private boolean isActive = true;
    public GameObject gameObject = null;

    public void start() { return; }
    public void update() { return; }
    public void render() { return; }

    public boolean isActive() {
        return this.isActive;
    }

    public void setActive(boolean active) {
        this.isActive = active;
        return;
    }

    public String tag() {
        return this.gameObject.tag();
    }


}
