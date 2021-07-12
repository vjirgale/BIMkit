

class Rule{
    constructor(ID, name, description, errorLevel){//, ecsCount){
        this.ID = ID;
        this.Valid = true;

        this.Name = name;
        this.Description = description;
        this.ErrorLevel = errorLevel;
        this.ExistentialClauses = {};
        this.LogicalExpression = {};
    }
    get id(){
        return this.ID;
    }
    set id(x){
        this.ID = x;
    }
    get valid(){
        return this.Valid;
    }
    set valid(x){
        this.Valid = x;
    }
    set name(x) {
        this.Name = x;
    }
    set description(x) {
        this.Description = x;
    }
    set errorLevel(x) {
        this.ErrorLevel = x;
    }
    set ecsCount(x){
        this.ECS_Count = x;
    }

    get existentialClauses(){
        return this.ExistentialClauses;
    }
    set existentialClauses(x) {
        this.ExistentialClauses = x;
    }
    set logicalExpression(x) {
        this.LogicalExpression = x;
    }
    toJSON(){
        return {
            ErrorLevel: this.ErrorLevel,
            ExistentialClauses: this.ExistentialClauses,
            LogicalExpression: this.LogicalExpression,
            Name: this.Name,
            Description: this.Description};
    }
}




