package components;

import ecs.Component;

public class Player extends Component {
    public String name = "";

    public void start() {
        System.out.println("Player started ");
        return;
    }

    public void update() {
        System.out.println("Player updated");
        return;
    }

    public void render() {
        System.out.println("Player rendered");
        return;
    }
}
