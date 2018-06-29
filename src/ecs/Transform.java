package ecs;

import ecs.*;

import java.util.ArrayList;

public class Transform extends Component {

    public Transform parent = null;
    public ArrayList<Transform> children = new ArrayList<>();
    public Vec2i position = new Vec2i();

    public void setParent(Transform transform) {
        this.parent = transform;
        this.parent.children.add(transform);
        return;
    }

    public void translate(int dx, int dy) {
        this.position.x += dx;
        this.position.y += dy;
        return;
    }

}
