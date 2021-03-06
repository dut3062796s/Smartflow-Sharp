﻿/********************************************************************
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 ********************************************************************
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Smartflow.Dapper;
using Smartflow.Enums;

namespace Smartflow.Elements
{
    [XmlInclude(typeof(List<Transition>))]
    [XmlInclude(typeof(List<Group>))]
    public class Node : ASTNode
    {
        private WorkflowNodeCategeory _nodeType = WorkflowNodeCategeory.Normal;

        public override WorkflowNodeCategeory NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        [XmlElement(ElementName = "group")]
        public virtual List<Group> Groups
        {
            get;
            set;
        }

        internal override void Persistent()
        {
            base.Persistent();

            if (Transitions != null)
            {
                foreach (Transition transition in Transitions)
                {
                    transition.RNID = this.NID;
                    transition.ORIGIN = this.IDENTIFICATION;
                    transition.INSTANCEID = INSTANCEID;
                    transition.Persistent();
                }
            }

            if (Groups!= null)
            {
                foreach (Group r in Groups)
                {
                    r.RNID = this.NID;
                    r.INSTANCEID = INSTANCEID;
                    r.Persistent();
                }
            }
        }

        public ASTNode GetNode(long IDENTIFICATION)
        {
            string query = "SELECT * FROM T_NODE WHERE IDENTIFICATION=@IDENTIFICATION AND INSTANCEID=@INSTANCEID";
            ASTNode node = Connection.Query<ASTNode>(query, new
            {
                IDENTIFICATION = IDENTIFICATION,
                INSTANCEID = INSTANCEID

            }).FirstOrDefault();

            return node;
        }
    }
}
