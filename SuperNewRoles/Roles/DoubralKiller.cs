﻿using Hazel;
using SuperNewRoles.CustomRPC;
using SuperNewRoles.Mode;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using SuperNewRoles.Buttons;
using System.Linq;


namespace SuperNewRoles.Roles
{
    public static class DoubralKiller
    {
        public static void resetNormalCoolDown()
        {
            HudManagerStartPatch.DoubralKillerNormalKillButton.MaxTimer = RoleClass.DoubralKiller.KillTime;
            HudManagerStartPatch.DoubralKillerNormalKillButton.Timer = RoleClass.DoubralKiller.KillTime;
        }
        public static void resetSecondCoolDown()
        {
            HudManagerStartPatch.DoubralKillerSecondKillButton.MaxTimer = RoleClass.DoubralKiller.SecondKillTime;
            HudManagerStartPatch.DoubralKillerSecondKillButton.Timer = RoleClass.DoubralKiller.SecondKillTime;
        }
        public static void EndMeeting()
        {
            resetSecondCoolDown();
        }
        public static void FixedUpdate()
        {
            HudManager.Instance.KillButton.gameObject.SetActive(false);
            bool IsViewButtonText = false;
            if (!RoleClass.IsMeeting)
            {
                if (!RoleClass.IsMeeting)
                {
                    if (ModeHandler.isMode(ModeId.Default))
                    {
                        if (PlayerControl.LocalPlayer.isRole(RoleId.DoubralKiller) && RoleClass.DoubralKiller.IsSuicideViewL && RoleClass.DoubralKiller.IsSuicideViewR)
                        {
                            IsViewButtonText = true;
                            RoleClass.DoubralKiller.SuicideLTime -= Time.fixedDeltaTime;
                            RoleClass.DoubralKiller.SuicideRTime -= Time.fixedDeltaTime;
                            if (RoleClass.DoubralKiller.SuicideLTime <= 0)
                            {
                                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomRPC.RPCMurderPlayer, SendOption.Reliable, -1);
                                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                                writer.Write(byte.MaxValue);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.RPCMurderPlayer(PlayerControl.LocalPlayer.PlayerId, PlayerControl.LocalPlayer.PlayerId, byte.MaxValue);
                            }
                            if (RoleClass.DoubralKiller.SuicideRTime <= 0)
                            {
                                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CustomRPC.RPCMurderPlayer, SendOption.Reliable, -1);
                                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                                writer.Write(byte.MaxValue);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.RPCMurderPlayer(PlayerControl.LocalPlayer.PlayerId, PlayerControl.LocalPlayer.PlayerId, byte.MaxValue);
                            }
                        }
                    }
                }
            }
            if (IsViewButtonText && RoleClass.DoubralKiller.IsSuicideViewL && RoleClass.DoubralKiller.IsSuicideViewR && PlayerControl.LocalPlayer.isAlive())
            {
                RoleClass.DoubralKiller.SuicideKillLText.text = string.Format(ModTranslation.getString("DoubralKillerSuicideLText"), ((int)RoleClass.DoubralKiller.SuicideLTime) + 1);
                RoleClass.DoubralKiller.SuicideKillRText.text = string.Format(ModTranslation.getString("DoubralKillerSuicideRText"), ((int)RoleClass.DoubralKiller.SuicideRTime) + 1);
            }
            else
            {
                if (RoleClass.DoubralKiller.SuicideKillLText.text != "")
                {
                    RoleClass.DoubralKiller.SuicideKillLText.text = "";
                }
                if (RoleClass.DoubralKiller.SuicideKillRText.text != "")
                {
                    RoleClass.DoubralKiller.SuicideKillRText.text = "";
                }
            }
        }
        public static void MurderPlayer(PlayerControl __instance, PlayerControl target)
        {
            if (__instance.isRole(RoleId.DoubralKiller))
            {
                if (__instance.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                {
                    RoleClass.DoubralKiller.SuicideLTime = RoleClass.DoubralKiller.SuicideDefaultLTime;
                    RoleClass.DoubralKiller.IsSuicideViewL = true;
                    RoleClass.DoubralKiller.SuicideRTime = RoleClass.DoubralKiller.SuicideDefaultRTime;
                    RoleClass.DoubralKiller.IsSuicideViewR = true;
                }
                RoleClass.DoubralKiller.IsSuicideViewsL[__instance.PlayerId] = true;
                if (ModeHandler.isMode(ModeId.SuperHostRoles))
                {
                    RoleClass.DoubralKiller.SuicideTimersL[__instance.PlayerId] = RoleClass.DoubralKiller.SuicideDefaultLTime;
                }
                RoleClass.DoubralKiller.IsSuicideViewsR[__instance.PlayerId] = true;
                if (ModeHandler.isMode(ModeId.SuperHostRoles))
                {
                    RoleClass.DoubralKiller.SuicideTimersR[__instance.PlayerId] = RoleClass.DoubralKiller.SuicideDefaultRTime;
                }
                else if (ModeHandler.isMode(ModeId.Default))
                {
                    if (__instance.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                    {
                        __instance.SetKillTimerUnchecked(RoleClass.DoubralKiller.KillTime);
                        __instance.SetKillTimerUnchecked(RoleClass.DoubralKiller.SecondKillTime);
                        RoleClass.DoubralKiller.SuicideLTime = RoleClass.DoubralKiller.SuicideDefaultLTime;
                        RoleClass.DoubralKiller.SuicideRTime = RoleClass.DoubralKiller.SuicideDefaultRTime;
                    }
                }
            }
        }
        public static void WrapUp()
        {
            if (RoleClass.DoubralKiller.IsMeetingReset)
            {
                RoleClass.DoubralKiller.SuicideLTime = RoleClass.DoubralKiller.SuicideDefaultLTime;
                foreach (PlayerControl p in RoleClass.DoubralKiller.DoubralKillerPlayer)
                {
                    if (RoleClass.DoubralKiller.SuicideTimersL.ContainsKey(p.PlayerId))
                    {
                        RoleClass.DoubralKiller.SuicideTimersL[p.PlayerId] = RoleClass.DoubralKiller.SuicideDefaultLTime;
                    }
                }
                RoleClass.DoubralKiller.SuicideRTime = RoleClass.DoubralKiller.SuicideDefaultRTime;
                foreach (PlayerControl p in RoleClass.DoubralKiller.DoubralKillerPlayer)
                {
                    if (RoleClass.DoubralKiller.SuicideTimersR.ContainsKey(p.PlayerId))
                    {
                        RoleClass.DoubralKiller.SuicideTimersR[p.PlayerId] = RoleClass.DoubralKiller.SuicideDefaultRTime;
                    }
                }
            }
        }
        public class DoubralKillerFixedPatch
        {
            public static PlayerControl DoubralKillersetTarget(bool onlyCrewmates = false, bool targetPlayersInVents = false, List<PlayerControl> untargetablePlayers = null, PlayerControl targetingPlayer = null)
            {
                PlayerControl result = null;
                float num = GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)];
                if (!ShipStatus.Instance) return result;
                if (targetingPlayer == null) targetingPlayer = PlayerControl.LocalPlayer;
                if (targetingPlayer.Data.IsDead || targetingPlayer.inVent) return result;

                if (untargetablePlayers == null)
                {
                    untargetablePlayers = new List<PlayerControl>();
                }

                Vector2 truePosition = targetingPlayer.GetTruePosition();
                Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
                for (int i = 0; i < allPlayers.Count; i++)
                {
                    GameData.PlayerInfo playerInfo = allPlayers[i];
                    if (!playerInfo.Disconnected && playerInfo.PlayerId != targetingPlayer.PlayerId && playerInfo.Object.isAlive())
                    {
                        PlayerControl @object = playerInfo.Object;
                        if (untargetablePlayers.Any(x => x == @object))
                        {
                            // if that player is not targetable: skip check
                            continue;
                        }

                        if (@object && (!@object.inVent || targetPlayersInVents))
                        {
                            Vector2 vector = @object.GetTruePosition() - truePosition;
                            float magnitude = vector.magnitude;
                            if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                            {
                                result = @object;
                                num = magnitude;
                            }
                        }
                    }
                }
                return result;
            }
        }
    }
}