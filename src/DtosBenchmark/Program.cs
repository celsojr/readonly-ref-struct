using Common.Class;
using Common.Struct;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

namespace DtosBenchmark
{
    public class Program
    {
        static void Main() => BenchmarkRunner.Run<TypeCompare>();
    }

    [CoreJob, MemoryDiagnoser]
    public class TypeCompare
    {
        [Benchmark(Baseline = true)]
        public DrawingDtoClass TheDrawingDtoClass() => new DrawingDtoClass();

        [Benchmark]
        public DrawingDtoStruct TheDrawingDtoStruct() => new DrawingDtoStruct();
    }
}
