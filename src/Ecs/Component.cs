using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs
{
    public class Component
    {
        private bool isActive = true;
        public GameObject gameObject = null;

        public virtual void Start()
        {
            return;
        }

        public virtual void Update()
        {
            return;
        }

        public virtual void Render()
        {
            return;
        }

        public bool IsActive()
        {
            return this.isActive;
        }

        public void SetActive(bool active)
        {
            this.isActive = active;
            return;
        }

        public String Tag()
        {
            return this.gameObject.Tag();
        }


    }

}
