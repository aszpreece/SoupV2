namespace SoupV2.NEAT.mutation
{
    public class InnovationIdManager
    {

        private int ConnectionId;
        private int NodeId;

        public InnovationIdManager(int initialConnectionId, int initialNodeId)
        {
            ConnectionId = initialConnectionId;
            NodeId = initialNodeId;
        }

        public int GetNextNodeInnovationId()
        {
            NodeId++;
            return NodeId;
        }

        public int GetNextConnectionInnovationId()
        {
            ConnectionId++;
            return ConnectionId;
        }

        public int PeekNextNodeInnovationId()
        {
            return NodeId+1;
        }

        public int PeekNextConnectionInnovationId()
        {
            return ConnectionId+1;
        }
    }
}