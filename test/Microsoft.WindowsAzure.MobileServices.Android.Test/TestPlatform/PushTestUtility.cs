﻿// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using Microsoft.WindowsAzure.MobileServices.TestFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.MobileServices.Test
{
    class PushTestUtility : IPushTestUtility
    {
        private const string DefaultChannelUri =
            "https://bn1.notify.windows.com/?token=AgYAAADs42685sa5PFCEy82eYpuG8WCPB098AWHnwR8kNRQLwUwf%2f9p%2fy0r82m4hxrLSQ%2bfl5aNlSk99E4jrhEatfsWgyutFzqQxHcLk0Xun3mufO2G%2fb2b%2ftjQjCjVcBESjWvY%3d";
        const string BodyTemplate = "<toast><visual><binding template=\"ToastText01\"><text id=\"1\">$(message)</text></binding></visual></toast>";
        const string DefaultToastTemplateName = "templateForToastWns";
        readonly string[] DefaultTags = { "fooWns", "barWns" };        

        public string GetPushHandle()
        {
            return DefaultChannelUri;
        }

        public string GetUpdatedPushHandle()
        {
            return DefaultChannelUri.Replace('A', 'B');
        }

        public Registration GetTemplateRegistrationForToast()
        {
            var channel = GetPushHandle();
            return new GcmTemplateRegistration(channel, BodyTemplate, DefaultToastTemplateName, DefaultTags);
        }

        public void ValidateTemplateRegistration(Registration registration)
        {
            var gcmTemplateRegistration = (GcmTemplateRegistration)registration;
            Assert.AreEqual(gcmTemplateRegistration.BodyTemplate, BodyTemplate);            

            foreach (string tag in DefaultTags)
            {
                Assert.IsTrue(registration.Tags.Contains(tag));
            }

            Assert.AreEqual(gcmTemplateRegistration.Name, DefaultToastTemplateName);
            Assert.AreEqual(gcmTemplateRegistration.TemplateName, DefaultToastTemplateName);
        }

        public void ValidateTemplateRegistrationBeforeRegister(Registration registration)
        {
            ValidateTemplateRegistration(registration);
            Assert.AreEqual(registration.Tags.Count, DefaultTags.Length);
            Assert.IsNull(registration.RegistrationId);
        }

        public void ValidateTemplateRegistrationAfterRegister(Registration registration, string zumoInstallationId)
        {
            ValidateTemplateRegistration(registration);
            Assert.IsNotNull(registration.RegistrationId);
            // TODO: Uncomment when .Net Runtime implements installationID
            //Assert.IsTrue(registration.Tags.Contains(zumoInstallationId));
            Assert.AreEqual(registration.Tags.Count, DefaultTags.Length + 1);
        }

        public Registration GetNewNativeRegistration(string deviceId, IEnumerable<string> tags)
        {
            return new GcmRegistration(deviceId, tags);
        }        

        public Registration GetNewTemplateRegistration(string deviceId, string bodyTemplate, string templateName)
        {
            return new GcmTemplateRegistration(deviceId, bodyTemplate, templateName);
        }
    }
}