using DotnetExpertAdvisor.Architecture.Tests;

ContractSmokeTests.ValidateCoreRecords();
ContractSmokeTests.ValidateErrorTypePreservesCandidates();
ContractSmokeTests.ValidateBinderStatuses();
ContractSmokeTests.ValidateConversionClassifier();
ContractSmokeTests.ValidateMemberLookup();
ContractSmokeTests.ValidateLanguageCapabilities();
Console.WriteLine("Architecture smoke checks passed.");
