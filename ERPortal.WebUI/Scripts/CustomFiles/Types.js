export const STATUSERROR = {
    ERROR: "ERROR",
    NOCOMMENTREC: "NCR",
    QUERYREPSUCC: "Query Reply Successfully"
};
export const PATTERNS = { DateFormate: /Date\(([^)]+)\)/ };
export const YesNo = {
    Yes: "0",
    No: "1"
};
export const HydrocarbonType = {
    Conventional: "0",
    UnConventional: "1"
};
export const HydrocarbonMethodProposed = {
    Oil: "0",
    Gas: "1",
    UHC: "2"
};
export const ImplementationType = {
    EOR: "0",
    IOR: "1",
    EGR: "2",
    IGR: "3",
    UHC: "4"
};
export const StyleClass = {
    UHC: 'uhc',
    GAS: 'gas',
    OIL: 'oil',

};

export const statusDeck = {
    statusAll: "All",
    statusAP: "Approved",
    statusNA: "Application Submitted",
    statusUP: "Under Proccessing",
    statusPWM: "Pending With Me",
    statusRJ: "Reject"
}
export const filterData = {
    type: "Status",
    statustext: statusDeck.statusAll
};

export const AlertColors = {
    Primary: "alert-primary",
    Info: "alert-info",
    Danger: "alert-danger",
    Dark: "alert-dark",
    Secondary: "alert-secondary",
    Success: "alert-success",
    Warning: "alert-warning"
};
export const DateDiff = {
    diffYear: 0,
    diffmonth: 0,
    diffdate: 0
};
export const ReturnData = {
    Status: null,
    Msg: "",
    Mandatory: []
};

export const UserRole = {
    CER: "Consultant Enhanced Recovery",
    Coord: "Coordinator",
}
export const ShowHide = {
    Show: "Show",
    Hide:"Hide",
};
