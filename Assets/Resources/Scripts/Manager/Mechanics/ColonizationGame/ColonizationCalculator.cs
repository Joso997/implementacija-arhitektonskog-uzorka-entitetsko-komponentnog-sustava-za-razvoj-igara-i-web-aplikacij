using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Mechanics.ColonizationGame
{
    public abstract class ColonizationCalculator
    {
        public abstract int CalculateScore(StasisData.ChamberData data);
        public virtual int CalculateScore(bool hasBothGenders = false)
        {
            return SetPercentage();
        }
        protected int amountOfAlive;
        protected int amountOfDead;
        protected virtual int ReqAmountOfAlive { get => 0;}
        protected virtual int ReqAmountOfDead { get => 0; }
        public int GetPercentage { get; private set; }
        protected int SetPercentage()
        {
            GetPercentage = (ReqAmountOfAlive > 0 ? ((amountOfAlive * 100) / ReqAmountOfAlive) : 100) + (ReqAmountOfDead > 0 ? ((amountOfDead * 100) / ReqAmountOfDead) : 100);
            return GetPercentage;
        }

        public abstract string nameOfScenario { get; }

    }
    public abstract class Scenarios
    {
        protected List<ColonizationCalculator> colonizationCalculators;
        protected int Score = 0;
        protected bool noScenarioFound = true;
        protected Queue<ColonizationCalculator> currentScenarios;
        public abstract string Calculate(Dictionary<int, StasisData.ChamberData>.ValueCollection data);
    }

    public class ReproductionScenarios : Scenarios
    {
        bool HasBothGenders = false;
        public ReproductionScenarios()
        {
            currentScenarios = new Queue<ColonizationCalculator>();
            colonizationCalculators = new List<ColonizationCalculator>(){ new PBomb(), new Cloning(), new Adaptation(), new Cyborg(), new Infertile(), new HCCrisis(), new Starvation() };
        }
        public override string Calculate(Dictionary<int, StasisData.ChamberData>.ValueCollection data)
        {
            int maleGenders = 0;
            int femaleGenders = 0;
            foreach (StasisData.ChamberData extraData in data)
            {
                if (extraData.InteractableObject.Stats[Events.Type.InteractableObjectStatEnum.Health].Points == 0)
                {
                    extraData.ChamberStatus = StasisData.ChamberStatusEnum.dead;
                }
                    
                if (extraData.ChamberStatus == StasisData.ChamberStatusEnum.alive && extraData.Gender == StasisData.GenderEnum.female)
                    femaleGenders++;
                else if (extraData.ChamberStatus == StasisData.ChamberStatusEnum.alive && extraData.Gender == StasisData.GenderEnum.male)
                    maleGenders++;
            }
            if (femaleGenders > 0 && maleGenders > 0)
                HasBothGenders = true;
            List<ColonizationCalculator> firstRun = colonizationCalculators.ToList();
            firstRun.Remove(firstRun.Single(x => x is Cloning));
            firstRun.Remove(firstRun.Single(x => x is Adaptation));
            firstRun.Remove(firstRun.Single(x => x is Cyborg));
            foreach (var temp in firstRun)
            {
                foreach (StasisData.ChamberData extraData in data)
                {
                    temp.CalculateScore(extraData);
                }
                int tempScore = temp.CalculateScore(HasBothGenders);
                if (tempScore >= 200)
                {
                    currentScenarios.Enqueue(temp);
                    if (temp == colonizationCalculators[0])
                    {
                        noScenarioFound = false;
                        break;
                    }                       
                }
            }
            if (noScenarioFound)
            {
                var secondRun = colonizationCalculators.ToList();
                firstRun.Remove(firstRun.Single(x => x is Starvation));
                firstRun.Remove(firstRun.Single(x => x is HCCrisis));
                firstRun.Remove(firstRun.Single(x => x is Infertile));
                firstRun.Remove(firstRun.Single(x => x is PBomb));
                ColonizationCalculator scenario = new PBomb();
                foreach (var temp in secondRun)
                {
                    foreach (StasisData.ChamberData extraData in data)
                    {
                        temp.CalculateScore(extraData);
                    }
                    int tempScore = temp.CalculateScore(HasBothGenders);
                    if (Score < tempScore)
                    {
                        Score = tempScore;
                        scenario = temp;
                    }
                }
                currentScenarios.Enqueue(scenario);
            }
            
            return currentScenarios.Dequeue().nameOfScenario;
        }
        #region Reproduction Types
        class PBomb : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Population Bomb"; }
            protected override int ReqAmountOfAlive { get => 5; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Farmer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Culture && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Terraformer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                return SetPercentage();
            }
            public override int CalculateScore(bool hasBothGenders)
            {
                if (hasBothGenders)
                    amountOfAlive++;
                return SetPercentage();
            }
        }

        class Cloning : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Cloning"; }
            protected override int ReqAmountOfAlive { get => 3;}
            protected override int ReqAmountOfDead { get => 2; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Administrator && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Culture && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                return SetPercentage();
            }
            public override int CalculateScore(bool hasBothGenders)
            {
                if (!hasBothGenders)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class Adaptation : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Adaptation"; }
            protected override int ReqAmountOfAlive { get => 1; }
            protected override int ReqAmountOfDead { get => 2;}
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Terraformer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class Cyborg : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Cyborg"; }
            protected override int ReqAmountOfAlive { get => 1;  }
            protected override int ReqAmountOfDead { get => 3;  }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Farmer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Culture && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class Infertile : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Infertile"; }
            protected override int ReqAmountOfDead { get => 3;  }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
            public override int CalculateScore(bool hasBothGenders)
            {
                if (!hasBothGenders)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class HCCrisis : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Health Care Crysis"; }
            protected override int ReqAmountOfDead { get => 2; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class Starvation : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Starvation"; }
            protected override int ReqAmountOfDead { get => 2; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Farmer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        #endregion
    }
    public class WayOfLifeScenarios : Scenarios
    {
        public WayOfLifeScenarios()
        {
            currentScenarios = new Queue<ColonizationCalculator>();
            colonizationCalculators = new List<ColonizationCalculator>(){ new CGoal(), new OWays(), new NEden(), new UColonies(), new Metropolis(), new RWars(), new Inequality() };
        }
        public override string Calculate(Dictionary<int, StasisData.ChamberData>.ValueCollection data)
        {
            foreach (StasisData.ChamberData extraData in data)
            {
                if (extraData.InteractableObject.Stats[Events.Type.InteractableObjectStatEnum.Health].Points == 0)
                    extraData.ChamberStatus = StasisData.ChamberStatusEnum.dead;
            }
            var firstRun = colonizationCalculators.ToList();
            firstRun.RemoveRange(1, 4);
            foreach (var temp in firstRun)
            {
                foreach (StasisData.ChamberData extraData in data)
                {
                    temp.CalculateScore(extraData);
                }
                int tempScore = temp.CalculateScore();
                if (tempScore >= 200)
                {
                    currentScenarios.Enqueue(temp);
                    if (temp == colonizationCalculators[0])
                    {
                        noScenarioFound = false;
                        break;
                    }
                }
            }
            if (noScenarioFound)
            {
                var secondRun = colonizationCalculators.ToList();              
                secondRun.RemoveAt(6);
                secondRun.RemoveAt(5);
                secondRun.RemoveAt(0);
                ColonizationCalculator scenario = new CGoal();
                foreach (var temp in secondRun)
                {
                    foreach (StasisData.ChamberData extraData in data)
                    {
                        temp.CalculateScore(extraData);
                    }
                    int tempScore = temp.CalculateScore();
                    if (Score < tempScore)
                    {
                        Score = tempScore;
                        scenario = temp;
                    }
                }
                currentScenarios.Enqueue(scenario);
            }

            
            if(currentScenarios.Count > 0)
                return currentScenarios.Dequeue().nameOfScenario;
            else
                return currentScenarios.Dequeue().nameOfScenario;
        }
        #region WayOfLife Types
        class CGoal : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Common Goal"; }
            protected override int ReqAmountOfAlive { get => 6; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Architect && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Diplomat && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Administrator && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Culture && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                return SetPercentage();
            }
        }
        class OWays : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Old Ways"; }
            protected override int ReqAmountOfAlive { get => 1; }
            protected override int ReqAmountOfDead { get => 3; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Diplomat && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Administrator && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Culture && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class NEden : ColonizationCalculator
        {
            public override string nameOfScenario { get => "New Eden"; }
            protected override int ReqAmountOfAlive { get => 2; }
            protected override int ReqAmountOfDead { get => 3; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Farmer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Diplomat && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Economist && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Terraformer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                return SetPercentage();
            }
        }
        class UColonies : ColonizationCalculator
        {
            public override string nameOfScenario { get => "United Colonies"; }
            protected override int ReqAmountOfAlive { get => 3; }
            protected override int ReqAmountOfDead { get => 3; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Architect && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Builder && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Diplomat && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Economist && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Administrator && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Culture && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class Metropolis : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Metropolis"; }
            protected override int ReqAmountOfAlive { get => 4; }
            protected override int ReqAmountOfDead { get => 1; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Architect && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Builder && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Economist && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Administrator && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Terraformer && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class RWars : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Resource Wars"; }
            protected override int ReqAmountOfDead { get => 3; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Diplomat && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Economist && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Administrator && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                return SetPercentage();
            }
        }
        class Inequality : ColonizationCalculator
        {
            public override string nameOfScenario { get => "Inequality"; }
            protected override int ReqAmountOfAlive { get => 4; }
            protected override int ReqAmountOfDead { get => 2; }
            public override int CalculateScore(StasisData.ChamberData data)
            {
                if (data.JobReq == StasisData.JobTypeEnum.Farmer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Diplomat && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Economist && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Administrator && data.ChamberStatus == StasisData.ChamberStatusEnum.dead)
                    amountOfDead++;
                else if (data.JobReq == StasisData.JobTypeEnum.Engineer && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                else if (data.JobReq == StasisData.JobTypeEnum.Researcher && data.ChamberStatus == StasisData.ChamberStatusEnum.alive)
                    amountOfAlive++;
                return SetPercentage();
            }
        }
        #endregion
    }
}