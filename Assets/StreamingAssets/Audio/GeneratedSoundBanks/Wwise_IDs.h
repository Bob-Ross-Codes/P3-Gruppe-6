/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAY_PLYR_FOOTSTEPS = 673033979U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace AREASTATE
        {
            static const AkUniqueID GROUP = 2064552269U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace AREASTATE

        namespace GAMESTATUS
        {
            static const AkUniqueID GROUP = 1045871717U;

            namespace STATE
            {
                static const AkUniqueID INGAME = 984691642U;
                static const AkUniqueID INMENU = 3374585465U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace GAMESTATUS

        namespace PLAYERSTATE
        {
            static const AkUniqueID GROUP = 3285234865U;

            namespace STATE
            {
                static const AkUniqueID ALIVE = 655265632U;
                static const AkUniqueID DEAD = 2044049779U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace PLAYERSTATE

        namespace ROOMSTATE
        {
            static const AkUniqueID GROUP = 185713839U;

            namespace STATE
            {
                static const AkUniqueID INDOOR = 340398852U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID OUTDOOR = 144697359U;
            } // namespace STATE
        } // namespace ROOMSTATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace GROUNDMATERIALSWITCH
        {
            static const AkUniqueID GROUP = 1044534455U;

            namespace SWITCH
            {
                static const AkUniqueID CARPET = 2412606308U;
                static const AkUniqueID TILE = 2637588553U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace GROUNDMATERIALSWITCH

        namespace GROUNDWETNESSSWITCH
        {
            static const AkUniqueID GROUP = 3968804893U;

            namespace SWITCH
            {
                static const AkUniqueID DAMP = 1842026663U;
                static const AkUniqueID DRY = 630539344U;
                static const AkUniqueID SOAKED = 1905651656U;
                static const AkUniqueID WET = 1181096339U;
            } // namespace SWITCH
        } // namespace GROUNDWETNESSSWITCH

        namespace PLAYERHEALTH
        {
            static const AkUniqueID GROUP = 151362964U;

            namespace SWITCH
            {
                static const AkUniqueID FULLHEALTH = 2429688720U;
                static const AkUniqueID LOWHEALTH = 1017222595U;
                static const AkUniqueID NEARDEATH = 898449699U;
            } // namespace SWITCH
        } // namespace PLAYERHEALTH

        namespace PLAYERSPEEDSWITCH
        {
            static const AkUniqueID GROUP = 2051106367U;

            namespace SWITCH
            {
                static const AkUniqueID RUN = 712161704U;
                static const AkUniqueID SNEAK = 2884887403U;
                static const AkUniqueID SPRINT = 1296465089U;
                static const AkUniqueID WALK = 2108779966U;
            } // namespace SWITCH
        } // namespace PLAYERSPEEDSWITCH

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID RTPC_DISTANCE = 262290038U;
        static const AkUniqueID RTPC_FLYCOUNT = 1318719891U;
        static const AkUniqueID RTPC_GROUNDWETNESS = 870672907U;
        static const AkUniqueID RTPC_PLAYERSPEED = 2653406601U;
        static const AkUniqueID RTPC_RAINAMOUNT = 1294084109U;
        static const AkUniqueID RTPC_TIMEOFDAY = 257272959U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID PLAYER = 1069431850U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID _2DAMBIENCE = 309309195U;
        static const AkUniqueID _2DAMBIENTBEDS = 4152869693U;
        static const AkUniqueID _3DAMBIENCE = 1301074112U;
        static const AkUniqueID AMBIENTBEDS = 1182634443U;
        static const AkUniqueID AMBIENTMASTER = 1459460693U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID NPCMASTER = 2033911932U;
        static const AkUniqueID PLAYERCLOTH = 765206498U;
        static const AkUniqueID PLAYERFOOTSTEPS = 1681012287U;
        static const AkUniqueID PLAYERLOCOMOTION = 2343802269U;
        static const AkUniqueID PLAYERMASTER = 3538689948U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID LARGEROOM = 187046019U;
        static const AkUniqueID REVERBS = 3545700988U;
        static const AkUniqueID SILENTROOM = 845761135U;
        static const AkUniqueID SMALLROOM = 2933838247U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
