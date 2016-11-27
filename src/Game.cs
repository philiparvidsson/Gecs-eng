namespace PrimusGE {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using Components;
using Core;
using Graphics;
using Sound;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Game {
    /*-------------------------------------
     * NON-PUBLIC CONSTANTS
     *-----------------------------------*/

    private const double INV_DRAWS_PER_SEC = 1.0 / 60.0;

    private const double INV_UPDATES_PER_SEC = 1.0 / 120.0;

    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private bool m_Done;

    private IGraphicsMgr m_Graphics;

    private readonly Dictionary<Type, List<Action<IMessage>>> m_MessageHandlers;

    private readonly Queue<IMessage> m_MessageQueue;

    private Scene m_Scene;

    private ISoundMgr m_Sound;

    private Form m_Window;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public ISoundMgr Sound {
        get { return m_Sound; }
    }

    public IGraphicsMgr Graphics {
        get { return m_Graphics; }
    }

    public static Game Inst { get; } = new Game();

    public Scene Scene {
        get { return m_Scene; }
    }

    public Form Window {
        get { return m_Window; }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    private Game() {
        m_MessageHandlers = new Dictionary<Type, List<Action<IMessage>>>();
        m_MessageQueue    = new Queue<IMessage>();
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void EnterScene(Scene scene) {
        scene.Parent = m_Scene;
        m_Scene = scene;

        scene.Init();
    }

    public void Exit() {
        m_Done = true;
    }

    public void LeaveScene() {
        if (m_Scene == null) {
            return;
        }

        m_Scene.Cleanup();
        m_Scene = m_Scene.Parent;
    }

    public void OnMessage<T>(Action<IMessage> cb) where T: IMessage {
        OnMessage(typeof (T), cb);
    }

    public void OnMessage(Type type, Action<IMessage> cb) {
        List<Action<IMessage>> callbacks;
        if (!m_MessageHandlers.TryGetValue(type, out callbacks)) {
            callbacks = new List<Action<IMessage>>();
            m_MessageHandlers[type] = callbacks;
        }

        callbacks.Add(cb);
    }

    public void PostMessage(IMessage message) {
        m_MessageQueue.Enqueue(message);
    }

    public void Run(IGraphicsMgr graphics,
                    ISoundMgr    sound,
                    string       title,
                    int          width,
                    int          height,
                    Scene        scene)
    {
        Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

        m_Done = false;

        Init(graphics, sound, title, width, height);
        EnterScene(scene);

        var t1 = 0.0;
        var t2 = 0.0;
        var stopwatch = Stopwatch.StartNew();
        while (!m_Done) {
            Application.DoEvents();

            var dt = stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();

            t1 += dt;

            var done = false;
            while (!done) {
                done = true;

                if (t1 >= INV_UPDATES_PER_SEC) {
                    m_Scene.Update((float)INV_UPDATES_PER_SEC);
                    t1 -= INV_UPDATES_PER_SEC;
                    t2 += INV_UPDATES_PER_SEC;

                    done = false;
                }

                if (t2 >= INV_DRAWS_PER_SEC) {
                    Graphics.IsEnabled = t1 < INV_DRAWS_PER_SEC;

                    m_Scene.Draw((float)INV_DRAWS_PER_SEC);
                    t2 -= INV_DRAWS_PER_SEC;

                    done = false;
                }

                DispatchMessages();
            }

            if (m_Scene == null) {
                Exit();
            }
        }

        while (m_Scene != null) {
            LeaveScene();
        }

        Cleanup();
    }

    public Entity SetTimeout(Action cb, float time) {
        var timer = new Entity();

        timer.AddComponent(new LifetimeComponent { EndOfLife = cb,
                                                   Lifetime  = time });

        Scene.AddEntity(timer);
        return timer;
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private void Init(IGraphicsMgr graphics,
                      ISoundMgr    sound,
                      string       title,
                      int          width,
                      int          height)
    {
        m_Window = CreateWindow(title, width, height);

        graphics.Init(m_Window);
        sound.Init();
        m_Graphics = graphics;
        m_Sound    = sound;
    }

    private void Cleanup() {
        m_Graphics.Cleanup();
        m_Graphics = null;

        m_Window.Close();
        m_Window.Dispose();
        m_Window = null;
    }

    private Form CreateWindow(string title, int width, int height) {
        var form = new GameForm();

        form.Hide();

        form.FormClosed += (sender, e) => Exit();

        form.ClientSize = new Size(width, height);
        form.Text = title;

        return form;
    }

    private void DispatchMessages() {
        while (m_MessageQueue.Count > 0) {
            var msg = m_MessageQueue.Dequeue();

            List<Action<IMessage>> callbacks;
            if (!m_MessageHandlers.TryGetValue(msg.GetType(), out callbacks)) {
                continue;
            }

            foreach (var cb in callbacks) {
                cb(msg);
            }
        }
    }
}

}
