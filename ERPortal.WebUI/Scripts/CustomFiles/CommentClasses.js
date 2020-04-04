export class StatusData {
    constructor(ReturnData) {
        this.Status = ReturnData.Status;
        this.Msg = ReturnData.Msg;
        this.Mandatory = ReturnData.Mandatory
    }
}