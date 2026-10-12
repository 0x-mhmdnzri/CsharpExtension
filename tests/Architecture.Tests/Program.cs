using DotnetExpertAdvisor.Architecture.Tests;

ContractSmokeTests.ValidateCoreRecords();
ContractSmokeTests.ValidateErrorTypePreservesCandidates();
ContractSmokeTests.ValidateBinderStatuses();
Console.WriteLine("Architecture smoke checks passed.");
