namespace TibiaHeleper.Modules
{
    interface Module
    {
        void Run();
        bool working { get; set; }
        bool stopped { get; set; }
    }
}
