namespace StakeKnife.BackEnd

open System
open NodaMoney

module Entities =
    type Address = {Line1:string; Line2: string; City: string; State: string; Zip: string}

    type User = {Name: string; Email: string;}

    type AuditEntry = {User: User; Timestamp: DateTime; Action: string;}

    type FalloffCurve = List<decimal>

    type Stakeholder = {Name: string; Address: Address;}

    type Share = {Value: int; Reason: string; FromDate: DateTime; EndDate: DateTime; Falloff: FalloffCurve; Stakeholder: Stakeholder}
    
    type Bucket = {
        Value: decimal; Shares: List<Share>
    } with
        member this.TotalShares = this.Shares |> Seq.sumBy(fun share -> share.Value)

    type Project = {
        Name: string;
        Buckets: List<Bucket>;
    } with
        member this.TotalShares = this.Buckets |> Seq.sumBy(fun bucket -> bucket.TotalShares)

    type Studio = {Name: string; Projects: List<Project>}

    type PaymentStatus =
        | Created
        | AwaitingApproval
        | Approved
        | Sent
        | Received
    
    //placeholder for demonstration purposes, these should not be mutable
    type Payment = {
        mutable Project: Project;
        mutable Amount: Money;
        mutable Stakeholder: Stakeholder;
        mutable Status: PaymentStatus
    } with
        member this.set (project) = this.Project <- project
        member this.set (amount) = this.Amount <- amount
        member this.set (stakeholder) = this.Stakeholder <- stakeholder
        member this.set (status) = this.Status <- status
    
    type PaymentEvent = { Date: DateTime; Payments: List<Payment>}
    