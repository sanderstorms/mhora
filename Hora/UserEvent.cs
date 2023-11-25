/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.Serialization;
using mhora.Calculation;
using mhora.Settings;

namespace mhora.Hora
{
    [Serializable]
    public class UserEvent : MhoraSerializableOptions, ICloneable, ISerializable
    {
        private string mEventDesc;

        private string mEventName;
        private Moment mEventTime;
        private bool   mWorkWithEvent;

        protected UserEvent(SerializationInfo info, StreamingContext context)
            : this()
        {
            Constructor(GetType(), info, context);
        }

        public UserEvent()
        {
            EventName     = "Some Event";
            EventTime     = new Moment();
            WorkWithEvent = true;
        }

        public string EventName
        {
            get =>
                mEventName;
            set =>
                mEventName = value;
        }

        [Editor(typeof(UIStringTypeEditor), typeof(UITypeEditor))]
        public string EventDesc
        {
            get =>
                mEventDesc;
            set =>
                mEventDesc = value;
        }

        public Moment EventTime
        {
            get =>
                mEventTime;
            set =>
                mEventTime = value;
        }

        public bool WorkWithEvent
        {
            get =>
                mWorkWithEvent;
            set =>
                mWorkWithEvent = value;
        }

        public object Clone()
        {
            var ue = new UserEvent();
            ue.EventName     = EventName;
            ue.EventTime     = EventTime;
            ue.WorkWithEvent = WorkWithEvent;
            ue.EventDesc     = EventDesc;
            return ue;
        }

        void ISerializable.GetObjectData(
            SerializationInfo info,
            StreamingContext  context)
        {
            GetObjectData(GetType(), info, context);
        }

        public override string ToString()
        {
            var ret = string.Empty;

            if (WorkWithEvent)
            {
                ret += "* ";
            }

            ret += EventName + ": " + EventTime;
            return ret;
        }
    }
}