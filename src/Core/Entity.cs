namespace GecsEng.Core {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Entity {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private readonly Dictionary<Type, object> m_Components;

    private static int s_NextID = 1;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int ID { get; }
    public Scene Scene { get; internal set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Entity() {
        m_Components = new Dictionary<Type, object>();

        ID = Interlocked.Increment(ref s_NextID);
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public T AddComponent<T>(T component) where T: class {
        m_Components.Add(component.GetType(),  component);
        return component;
    }

    public void AddComponents(params object[] components) {
        foreach (var component in components) {
            AddComponent(component);
        }
    }

    public void Destroy() {
        if (Scene != null) {
            Scene.RemoveEntity(ID);
        }
    }

    public T GetComponent<T>() {
        return (T)GetComponent(typeof (T));
    }

    public object GetComponent(Type type) {
        object component;
        m_Components.TryGetValue(type, out component);
        return component;
    }

    public object[] GetComponents() {
        return new List<object>(m_Components.Values).ToArray();
    }

    public bool HasComponent<T>() {
        return HasComponent(typeof (T));
    }

    public bool HasComponent(Type type) {
        return m_Components.ContainsKey(type);
    }

    public bool RemoveComponent<T>() {
        return RemoveComponent(typeof (T));
    }

    public bool RemoveComponent(Type type) {
        return m_Components.Remove(type);
    }
}

}
