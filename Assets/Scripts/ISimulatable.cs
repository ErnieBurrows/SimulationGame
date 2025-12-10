using UnityEngine;

public interface ISimulatable 
{
     // Called once per simulation tick. deltaTime is the simulation time step.
    void Simulate(float deltaTime);
}
