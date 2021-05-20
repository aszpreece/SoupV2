using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Brain.BrainTypes
{
    class HunterScriptBrainPhenotype : AbstractBrain
    {

        private Dictionary<string, float> _inputs = new Dictionary<string, float>();
        private Dictionary<string, float> _outputs = new Dictionary<string, float>();

       // {"wheelLeft", "MovementControlComponent.WishWheelLeftForce" },
       //{"wheelRight", "MovementControlComponent.WishWheelRightForce" },
       //{ "reproduce", "ReproductionComponent.Reproduce" },
       //{ "red", "VisibleColourComponent.R" },
       //{ "green", "VisibleColourComponent.G" },
       //{ "blue", "VisibleColourComponent.B" },
       //{ "attack", "weapon.WeaponComponent.Activation" }

        /// <summary>
        /// output ids for the different parts of the predator we care about
        /// </summary>
        string wheelLeft = "wheelLeft";
        string wheelRight = "wheelRight";

        string weapon = "attack";
        string suicide = "suicide";

        // inputs from eyes for eye 2 (assumed to be left) and eye 1 (assumed to be right)
        // eye r g bs

        string eyeLeftR = "eye2R";
        string eyeLeftG = "eye2G";
        string eyeLeftB = "eye2B";
        string eyeRightR = "eye1R";
        string eyeRightG = "eye1G";
        string eyeRightB = "eye1B";

        string weaponHit = "weaponhit";

        public HunterScriptBrainPhenotype(HunterScriptBrainGenotype genotype)
        {
            // Add nodes for inputs and outputs
            foreach (string namedInput in genotype.Inputs)
            {
                this._inputs.Add(namedInput, 0);
            }
            foreach (string namedOutput in genotype.Outputs)
            {
                this._inputs.Add(namedOutput, 0);
            }
        }

        Random r = new Random();
        // false = left, true = right
        bool currentDirection = false;
        public override void Calculate()
        {
            _outputs[suicide] = 0f;

            // if we hit a prey, then retreat
            if (_inputs[weaponHit] > 0 )
            {
                _outputs[suicide] = 1.0f;
            }
            // steer whichever way has the most activity
            // default is to move in a rough circle
            // randonly switch direction
            if (r.NextDouble() > 0.99)
            {
                currentDirection = !currentDirection;
            }
            float targetWishSpeedLeft;
            float targetWishSpeedRight;
            // choose which direction to steer
            if (currentDirection)
            {
                // right
                targetWishSpeedLeft = 0.6f;
                targetWishSpeedRight = 0.45f;
            } else
            {
                //left
                targetWishSpeedLeft = 0.45f;
                targetWishSpeedRight = 0.6f;
            }

            float averageEyeLeft = _inputs[eyeLeftR] + _inputs[eyeLeftG] + _inputs[eyeLeftB];
            float averageEyeRight = _inputs[eyeRightR] + _inputs[eyeRightG] + _inputs[eyeRightB];
            averageEyeLeft /= 3;
            averageEyeRight /= 3;


            // steer left
            if (averageEyeLeft > 0 )
            {
                targetWishSpeedLeft += 1f;
                targetWishSpeedRight -= 0.7f;

            }
            // steer right
            if (averageEyeRight > 0)
            {
                targetWishSpeedRight += 1f;
                targetWishSpeedLeft -= 0.7f;
            }

            _outputs[wheelLeft] = Math.Clamp(targetWishSpeedLeft, -1, 1);
            _outputs[wheelRight] = Math.Clamp(targetWishSpeedRight, -1, 1);
            // always be attacking
            _outputs[weapon] = 1.0f;

        }

        public override float GetInput(string name)
        {
            return _inputs[name];
        }
        internal override float GetOutput(string name)
        {
            return _outputs[name];
        }

        public override void SetInput(string name, float value)
        {
            _inputs[name] = value;
        }

        public override void SetInput(string name, float[] value)
        {

        }

    }
}
