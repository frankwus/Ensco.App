﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Services
{
    public static class IrmaConstants
    {
        public enum IrmaPobModels{
            Resources,
            CrewChange,
            CrewFlightManifest,
            CrewFlightManifestView,
            CrewFlightManifestPob,
            RigPersonnel,
            RigPersonnelView,
            RigPersonnelTeams,
            BatchOnboardView,
            BatchOffboardView,
            CrewPobView,
            FlightManifestPobView,
            FlightParkingLotView,
            CurrentPobView,
            IndividualOnboardView,
            PobSummaryView,
            PobSummaryByCompanyType,
            FlightManifestUnassigned,
            Teams,
            TeamMembers,
            TeamView,
            TeamMemberView,
            ParkingLot,
            PobRigRequirements,
            PobRigSubRequirements,
            PobRigRequirementDocs,
            PobRigCompliance,
            LifeBoatCompliance,
            LifeBoatRosterList,
            RoomBedSummary,
            CrewBreakDown,
            LifeBoatBreakDown,
            CrewOnboardView,
            FlightOnboardView,
            RosterByLifeBoat,
            RosterByMusterStation,
            RosterFull,
            Approval,
            ApprovalView,
            RigPersonnelHistory,
            RigPersonnelHistoryView,
            CrewPobAll,
            FlightPobAll,
            CrewArrivalLog,
            CrewDepartureLog,
            CrewArrivalDepartureLog,
            FlightArrivalLog,
            FlightDepartureLog,
            FlightArrivalDepartureLog,
            PobSummaryArrivalReport,
            PobSummaryDepartureReport,
            TourMangement,
            EmergencyTeamReportView,
            RigFieldVisible,
            RigPersonnelArchive,
            RigComplianceUsers,
            RigCompliance,
            CrewChangeSearch,
            PobOnOffboardView,
            PobSummaryCounts,
            Emails,
            TourChange,
            PobHistory,
            CrewChangeApprovalView,
            Admin,
            AdminView,
            AdminRole,
            IsolationLock,
            TasksCapa,
            TasksJob,
            TasksTask,
            TasksCap,
            TasksTlc,
            TasksIsolation,
            ReconcilePassport,
            JobCode };
        public enum IrmaLookupLists {
            PobStatus,
            PobPlanningStatus,
            PobFlightStatus,
            Tour,
            LifeBoat,
            MusterStation,
            Room,
            RoomBed,
            CrewChange,
            FlightManifest,
            FlightManifestUnassigned,
            CrewPobAll,
            CrewPobArriving,
            RigCrew,
            ScheduleType,
            TeamType,
            RigPersonnel,
            TeamPersonnel,
            PersonnelType,
            CrewFlights,
            RotationSchedule,
            PobAdminRole,
            IsolationType };
        public enum ApprovalStatus {
            Open = 1,
            PendingApproval = 2,
            PendingVerification = 3,
            PendingReview = 4,
            Verified = 5,
            Reviewed = 6,
            Approved = 7,
            Rejected = 8,
            Closed = 9,
            Disabled = 10,
            Active = 11
        }

    }
}
