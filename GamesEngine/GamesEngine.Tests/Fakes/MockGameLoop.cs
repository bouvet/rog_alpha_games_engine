using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Object;
using GamesEngine.Service.GameLoop;

namespace GamesEngine.Tests.Fakes;

public class MockGameLoop : IGameLoop
{
    private IGame Game { get; }
    private ITime Time { get; }
    private long time = 0;

    public MockGameLoop(IGame game, ITime time)
    {
        Game = game;
        Time = time;
    }

    public event Action<ITime, IInterval>? UpdateTick;
    public event Action<IGameObject>? GameObjectPreUpdate;
    public event Action<IGameObject>? GameObjectPostUpdate;

    public void ProcessInput()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        ITime curTime = new MockTime(time);
        ITime newTime = new MockTime(time + Time.GetTime());
        IInterval deltaTime = new Interval(curTime, newTime);
        Game.SceneGraph.DynamicGameObject.GetValues().ForEach(gameObject =>
        {
            gameObject.Update(deltaTime, curTime);

            if (gameObject.Motion.GetAbsolute() > 0)
            {
                gameObject.UpdateMovement(deltaTime, curTime);
            }
        });
        time += Time.GetTime();
    }

    public void Render()
    {
        throw new NotImplementedException();
    }
}