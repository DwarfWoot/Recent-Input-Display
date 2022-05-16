using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using InControl;
using System.Reflection;

namespace Recent_input_display;

    public class Recent_input_display : Mod
    {
        internal static Recent_input_display Instance;

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        //public Recent_input_display() : base("Recent_input_display")
        //{
        //    Instance = this;
        //}

        public override void Initialize()
        {
            Log("Letting the Dwarves in");
            Instance = this;
            Log("The Dwarves are in 👀");

            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }
        new public string GetName() => "Recent Item Display ❤";
        public override string GetVersion() => "Unreleased 👀";

        // This is apparently using Reflection to give access to the Hero Actions from the vanilla game
        private readonly FieldInfo[] heroActionFields = typeof(HeroActions).GetFields(BindingFlags.Instance | BindingFlags.Public);
        
        //List to store Player inputs one time
        public List<PlayerAction> PlayerInputs = new List();

        private static class MyVars
        {
            public static string vLastAction;
            public static string[] strActions = new string[] 
            { 
                "left", 
                "right", 
                "up", 
                "down",
                "rs_up",
                "rs_down",
                "rs_left",
                "rs_right",
                "jump",
                "dash",
                "superDash",
                "dreamNail",
                "attack",
                "cast",
                "focus",
                "quickMap",
                "quickCast",
                "openInventory",
                "pause"
            };
            //public static var vActions = new HeroActions[] {HeroActions};
            public static int intDebugCounter;
            

        }

        public void OnHeroUpdate()
        {
            //foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            foreach (var heroActionField in heroActionFields)
            {
                string actionName = heroActionField.Name; //gets name of the field like focus/cast/attack

                if (MyVars.intDebugCounter < heroActionFields.Length)
                {
                    Log("> int intDebugCounter: " + MyVars.intDebugCounter);
                    Log("=> var heroActionField: " + heroActionField);
                    Log("==> str actionName: " + actionName);
                    MyVars.intDebugCounter++;
                }

                if (heroActionField.GetValue(InputHandler.Instance.inputActions) is PlayerAction playerAction)
                {
                    //do whatever you want with the "playerAction" here like you use normally
                    //also you can check/use the name of the thing you are using from the "actionName" variable above

                    //e.g.
                    if ((playerAction.IsPressed) & (actionName != MyVars.vLastAction))
                    {
                        for (int i = 0; i < MyVars.strActions.Length; i++)
                        {
                            if (MyVars.strActions[i] == actionName)
                            {
                                Log($"{actionName} was pressed");
                                MyVars.vLastAction = actionName;
                            }
                        }
                    }
                }
                /*
                if (heroActionField.GetValue(InputHandler.Instance.inputActions) is PlayerTwoAxisAction playerTwoAxisAction)
                {
                    //similar comment to above
                    //e.g.
                    if (playerTwoAxisAction.IsPressed)
                    {
                        Log($"{actionName} was pressed");
                        MyVars.vLastAction = actionName;
                    }
                }
                */
            }
        }
    }