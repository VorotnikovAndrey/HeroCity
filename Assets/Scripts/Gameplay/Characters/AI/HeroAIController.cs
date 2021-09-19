using System.Collections.Generic;
using Gameplay.Characters.AI.Behaviors;
using Gameplay.Characters.Models;
using Source;
using UnityEngine;
using Utils;

namespace Gameplay.Characters.AI
{
    public class HeroAIController : IAIController
    {
        private BaseCharacterModel _model;
        private List<AbstractAIBehavior> _possibleStates;
        private AbstractAIBehavior _currentState;

        public HeroAIController()
        {
            _possibleStates = new List<AbstractAIBehavior>
            {
                new FreeMovementAIBehavior(1),
                new WanderingAIBehavior(999),
            };
        }

        public void Initialize(BaseCharacterModel model)
        {
            _model = model;
        }

        public void DeInitialize()
        {
            _model = null;
        }

        public void Start()
        {
            _currentState = _possibleStates[1];
            _currentState.Initialize(_model);
            _currentState.Begin();
        }

        public void Stop()
        {
            _currentState.Interrupt();
            _currentState = null;
        }
    }
}