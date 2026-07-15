using LernTor.Core.Enums;

namespace LernTor.Core.Services;

/// <summary>
/// Zentrale Zuordnung von reinen Fach-Lernstufen zu ihrem Fach. News, Abschlussquiz und die
/// Meta-Stufen (Willkommen/Freigeschaltet) sind hier bewusst nicht enthalten, da sie keinem
/// einzelnen Fach zugeordnet sind. Einzige Quelle der Wahrheit für diese Zuordnung, damit
/// ProgressGateService (Core) und die Navigation (App) nicht unabhängig voneinander gepflegt
/// werden und dabei auseinanderlaufen können.
/// </summary>
public static class LearningStageSubjects
{
    public static readonly IReadOnlyDictionary<LearningStage, Subject> Map = new Dictionary<LearningStage, Subject>
    {
        [LearningStage.Mathematik] = Subject.Mathematik,
        [LearningStage.Deutsch] = Subject.Deutsch,
        [LearningStage.Tuerkisch] = Subject.Tuerkisch,
        [LearningStage.Englisch] = Subject.Englisch,
        [LearningStage.Biologie] = Subject.Biologie,
        [LearningStage.Chemie] = Subject.Chemie,
        [LearningStage.Physik] = Subject.Physik,
        [LearningStage.Geschichte] = Subject.Geschichte,
        [LearningStage.Gewi] = Subject.Gewi,
        [LearningStage.Politik] = Subject.Politik,
        [LearningStage.Geo] = Subject.Geo,
        [LearningStage.Ethik] = Subject.Ethik,
        [LearningStage.Kunst] = Subject.Kunst,
        [LearningStage.Musik] = Subject.Musik,
        [LearningStage.Itg] = Subject.Itg,
        [LearningStage.Tippen] = Subject.Tippen,
    };

    public static bool TryGetSubject(LearningStage stage, out Subject subject) => Map.TryGetValue(stage, out subject);
}
