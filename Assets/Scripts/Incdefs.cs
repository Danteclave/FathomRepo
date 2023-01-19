using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incdefs
{
    public static bool IsOfType<T>(GameObject gameObject)
    {
        return gameObject is not null && gameObject.GetComponent<T>() != null;
    }
}

namespace Utils
{
    public class Interpolator
    {
        public class Point
        {
            public float x;
            public float y;
            public Point(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public Interpolator AddPointOffset(float x, float offset)
        {
            points.Add(new Point(x, points[points.Count - 1].y));
            return this;
        }

        public Interpolator AddPoint(Point pt)
        {
            points.Add(pt);
            return this;
        }

        public Interpolator RemovePoint(int id)
        {
            if(id <= points.Count)
                points.RemoveAt(id);
            return this;
        }

        public Interpolator RemovePoint(Point pt)
        {
            points.RemoveAll(p => p.x == pt.x && p.y == pt.y);
            return this;
        }

        private List<Point> points;
        public List<Point> Points
        {
            get => points;
        }

        public Action<float> setter;
        public Func<float> getter;


        private float _resolution;
        public float Resolution
        {
            get => _resolution;
        }
        private float delta;

        public Interpolator(List<Point> points, float resolution, Func<float> getter, Action<float> setter)
        {
            _resolution = resolution;
            delta = 1 / resolution;
            if(points == null)
            {
                points = new List<Point>();
            }
            else
            {
                this.points = points;
            }
            this.setter = setter;
            this.getter = getter;
        }

        private int currentid;
        private int nextid;

        private Point current
        {
            get => points[currentid];
        }

        private Point next
        {
            get => points[nextid];
        }

        private bool updatePoints()
        {
            if((slope > 0 && getter() > next.x) || (slope < 0 && getter() < next.x))
            {
                currentid = ++nextid;
                if (currentid >= points.Count)
                {
                    return false;
                }
            }
            return true;
        }

        public float slope
        {
            get => (next.y-current.y)/(next.x-current.x);
        }

        public void step()
        {
            if(!updatePoints() || Resolution > 0)
            {
                return;
            }
            setter(getter() + slope);
        }

        public IEnumerator start()
        {
            if (!updatePoints() || Resolution <= 0)
            {
                yield return null;
            }
            while (!updatePoints())
            {
                setter(getter() + slope * delta);
                yield return new WaitForSeconds(delta);
            }
        }

        public bool Done
        {
            get => updatePoints();
        }

    }

    public class SetOnlyTrigger
    {
        protected bool _set;
        public void on()
        {
            _set = true;
        }
        
        public bool consume()
        {
            return !(_set ^= true);
        }
    }

    public class Trigger : SetOnlyTrigger
    {
        public void off()
        {
            _set = false;
        }
    }
}
