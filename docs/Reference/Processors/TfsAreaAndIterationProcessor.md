## Processors: TfsAreaAndIterationProcessor

>**_This documentation is for a preview version of the Azure DevOps Migration Tools._ If you are not using the preview version then please head over to the main [documentation](https://nkdagility.github.io/azure-devops-migration-tools).**

[Overview](.././index.md) > [Reference](../index.md) > [Processors](./index.md) > **TfsAreaAndIterationProcessor**

The `TfsAreaAndIterationProcessor` migrates all of the Area nd Iteraion paths.

### Options

| Parameter name         | Type    | Description                              | Default Value                            |
|------------------------|---------|------------------------------------------|------------------------------------------|
| Enabled | Boolean | If set to `true` then the processor will run. Set to `false` and the processor will not run. | missng XML code comments |
| PrefixProjectToNodes | Boolean | Prefix your iterations and areas with the project name. If you have enabled this in `NodeStructuresMigrationConfig` you must do it here too. | false |
| NodeBasePaths | String[] | missng XML code comments | missng XML code comments |
| ProcessorEnrichers | List | List of Enrichers that can be used to add more features to this processor. Only works with Native Processors and not legacy Processors. | missng XML code comments |
| Source | IEndpointOptions | This is the `IEndpoint` that will be used as the source of the Migration. Can be null for a write only processor. | missng XML code comments |
| Target | IEndpointOptions | This is the `IEndpoint` that will be used as the Target of the Migration. Can be null for a write only processor. | missng XML code comments |
| RefName | String | `Refname` will be used in the future to allow for using named Options without the need to copy all of the options. | missng XML code comments |


### Example JSON

```JSON
{
  "$type": "TfsAreaAndIterationProcessorOptions",
  "Enabled": false,
  "PrefixProjectToNodes": false,
  "NodeBasePaths": null,
  "ProcessorEnrichers": null,
  "Source": {
    "$type": "TfsEndpointOptions",
    "Organisation": "https://dev.azure.com/nkdagility-preview/",
    "Project": "sourceProject",
    "AuthenticationMode": "AccessToken",
    "AccessToken": "6i4jyylsadkjanjniaydxnjsi4zsz3qarxhl2y5ngzzffiqdostq",
    "ReflectedWorkItemIdField": "Custom.ReflectedWorkItemId",
    "LanguageMaps": {
      "$type": "TfsLanguageMapOptions",
      "AreaPath": "Area",
      "IterationPath": "Iteration"
    },
    "EndpointEnrichers": null
  },
  "Target": {
    "$type": "TfsEndpointOptions",
    "Organisation": "https://dev.azure.com/nkdagility-preview/",
    "Project": "targetProject",
    "AuthenticationMode": "AccessToken",
    "AccessToken": "6i4jyylsadkjanjniaydxnjsi4zsz3qarxhl2y5ngzzffiqdostq",
    "ReflectedWorkItemIdField": "Custom.ReflectedWorkItemId",
    "LanguageMaps": {
      "$type": "TfsLanguageMapOptions",
      "AreaPath": "Area",
      "IterationPath": "Iteration"
    },
    "EndpointEnrichers": null
  }
}
```