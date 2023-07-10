using GamesEngine.Service.Game;
using GamesEngine.Service.Game.Object;

namespace GamesEngine.Service.GameLoop;

public interface IGameLoop
{
    public event Action<ITime, IInterval> UpdateTick;
    public event Action<IGameObject> GameObjectPreUpdate;
    public event Action<IGameObject> GameObjectPostUpdate;

    public void ProcessInput();
    public void Update();
    public void Render();
}

public class GameLoop : IGameLoop
{
    private ITime lastUpdate = new Time();

    public GameLoop(IGame game)
    {
        Game = game;
    }

    private IGame Game { get; }

    public event Action<ITime, IInterval>? UpdateTick = delegate {  };
    public event Action<IGameObject>? GameObjectPreUpdate = delegate {  };
    public event Action<IGameObject>? GameObjectPostUpdate = delegate {  };

    public void ProcessInput()
    {
        throw new NotImplementedException();
    }

    public void Render()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        ITime currentTime = new Time();
        IInterval deltaTime = new Interval(lastUpdate, currentTime);
        UpdateTick.Invoke(currentTime, deltaTime);

        Game.SceneGraph.DynamicGameObject.GetValues().ForEach(gameObject =>
        {
            GameObjectPreUpdate.Invoke(gameObject);
            gameObject.Update(deltaTime, currentTime);

            if (gameObject.Motion != null && gameObject.Motion.GetAbsolute() > 0)
            {
                gameObject.UpdateMovement(deltaTime, currentTime);
            }

            if(gameObject is PlayerGameObject)
                GameObject.CollisionCheck(Game, gameObject);

            GameObjectPostUpdate.Invoke(gameObject);
        });
        lastUpdate = new Time();
    }
}