using DotnetExpertAdvisor.Architecture.Tests;

ContractSmokeTests.ValidateCoreRecords();
ContractSmokeTests.ValidateErrorTypePreservesCandidates();
ContractSmokeTests.ValidateBinderStatuses();
ContractSmokeTests.ValidateConversionClassifier();
Console.WriteLine("Architecture smoke checks passed.");
