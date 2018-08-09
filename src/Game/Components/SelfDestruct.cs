#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using Ecs;

namespace Game.Components
{
    public class SelfDestruct : Component
    {
        // Default => Destroy this game object after 1 second.
        public float destroyAfterSeconds = 1.0f;

        private float dt = 0.0f;

        public SelfDestruct(float time = 1.0f)
        {
            destroyAfterSeconds = time;
        }


        public override void Update()
        {
            dt += ((float)Time.deltaMs / 1000.0f);

            if (dt >= destroyAfterSeconds)
            {
                GameObject.Destroy(this.gameObject);
            }
            return;
        }

    }
}
