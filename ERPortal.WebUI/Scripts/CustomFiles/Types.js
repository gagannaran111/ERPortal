export class MyType {

    const STATUSERROR = { ERROR: "ERROR", NOCOMMENTREC: "NCR", QUERYREPSUCC: "Query Reply Successfully" };
    const PATTERNS = { DateFormate: /Date\(([^)]+)\)/ };


    const YesNo = { Yes: "0", No: "1" };
    const HydrocarbonType = { Conventional: "0", UnConventional: "1" };
    const StyleClass = { UHC: 'uhc', GAS: 'gas', OIL: 'oil' };
    const DivId = {
        uhcProdnMethodDiv: $("#uhcProdnMethodDiv"),
        MethodProposedDiv: $("#MethodProposedDiv"),
        ImplementaionTypeDiv: $("#ImplementaionTypeDiv")
    };
}