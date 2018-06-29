package ecs;

public class Component {
    private boolean isActive;
    public GameObject gameObject;

    void start() { return; }
    void update() { return; }
    void render() { return; }

    public boolean isActive() {
        return this.isActive;
    }

    public void setActive(boolean active) {
        this.isActive = active;
        return;
    }

    public String getTag() {
        return this.gameObject.getTag();
    }


}
