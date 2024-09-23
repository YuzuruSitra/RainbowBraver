public interface INPCController
{
    bool IsFreedom { get; set; }
    InnNPCMover InnNPCMover { get; }
    void FinWarpHandler(int roomNum);
}