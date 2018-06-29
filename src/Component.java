package javecs;

public class Component {
    private boolean isActive;

    void start() { return; }
    void update() { return; }
    void render() { return; }

    boolean isActive() {
        return this.isActive;
    }
}
