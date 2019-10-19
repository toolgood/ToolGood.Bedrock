using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.WwwRoot.Test
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("lib/jquery/LICENSE.txt")]
        public IActionResult lib_jquery_LICENSE_txt()
        {
            if (Request.Headers["If-None-Match"]  == "828D0378AE850B159C2FF139FD371F9B") {
                Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
                return StatusCode(304);
            }
            Response.Headers["Cache-Control"] = "max-age=315360000";
            Response.Headers["Etag"] = "828D0378AE850B159C2FF139FD371F9B";
            Response.Headers["Date"] = DateTime.Now.ToString("r");
            Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
            const string s = "Q29weXJpZ2h0IEpTIEZvdW5kYXRpb24gYW5kIG90aGVyIGNvbnRyaWJ1dG9ycywgaHR0cHM6Ly9qcy5mb3VuZGF0aW9uLw0KDQpUaGlzIHNvZnR3YXJlIGNvbnNpc3RzIG9mIHZvbHVudGFyeSBjb250cmlidXRpb25zIG1hZGUgYnkgbWFueQ0KaW5kaXZpZHVhbHMuIEZvciBleGFjdCBjb250cmlidXRpb24gaGlzdG9yeSwgc2VlIHRoZSByZXZpc2lvbiBoaXN0b3J5DQphdmFpbGFibGUgYXQgaHR0cHM6Ly9naXRodWIuY29tL2pxdWVyeS9qcXVlcnkNCg0KVGhlIGZvbGxvd2luZyBsaWNlbnNlIGFwcGxpZXMgdG8gYWxsIHBhcnRzIG9mIHRoaXMgc29mdHdhcmUgZXhjZXB0IGFzDQpkb2N1bWVudGVkIGJlbG93Og0KDQo9PT09DQoNClBlcm1pc3Npb24gaXMgaGVyZWJ5IGdyYW50ZWQsIGZyZWUgb2YgY2hhcmdlLCB0byBhbnkgcGVyc29uIG9idGFpbmluZw0KYSBjb3B5IG9mIHRoaXMgc29mdHdhcmUgYW5kIGFzc29jaWF0ZWQgZG9jdW1lbnRhdGlvbiBmaWxlcyAodGhlDQoiU29mdHdhcmUiKSwgdG8gZGVhbCBpbiB0aGUgU29mdHdhcmUgd2l0aG91dCByZXN0cmljdGlvbiwgaW5jbHVkaW5nDQp3aXRob3V0IGxpbWl0YXRpb24gdGhlIHJpZ2h0cyB0byB1c2UsIGNvcHksIG1vZGlmeSwgbWVyZ2UsIHB1Ymxpc2gsDQpkaXN0cmlidXRlLCBzdWJsaWNlbnNlLCBhbmQvb3Igc2VsbCBjb3BpZXMgb2YgdGhlIFNvZnR3YXJlLCBhbmQgdG8NCnBlcm1pdCBwZXJzb25zIHRvIHdob20gdGhlIFNvZnR3YXJlIGlzIGZ1cm5pc2hlZCB0byBkbyBzbywgc3ViamVjdCB0bw0KdGhlIGZvbGxvd2luZyBjb25kaXRpb25zOg0KDQpUaGUgYWJvdmUgY29weXJpZ2h0IG5vdGljZSBhbmQgdGhpcyBwZXJtaXNzaW9uIG5vdGljZSBzaGFsbCBiZQ0KaW5jbHVkZWQgaW4gYWxsIGNvcGllcyBvciBzdWJzdGFudGlhbCBwb3J0aW9ucyBvZiB0aGUgU29mdHdhcmUuDQoNClRIRSBTT0ZUV0FSRSBJUyBQUk9WSURFRCAiQVMgSVMiLCBXSVRIT1VUIFdBUlJBTlRZIE9GIEFOWSBLSU5ELA0KRVhQUkVTUyBPUiBJTVBMSUVELCBJTkNMVURJTkcgQlVUIE5PVCBMSU1JVEVEIFRPIFRIRSBXQVJSQU5USUVTIE9GDQpNRVJDSEFOVEFCSUxJVFksIEZJVE5FU1MgRk9SIEEgUEFSVElDVUxBUiBQVVJQT1NFIEFORA0KTk9OSU5GUklOR0VNRU5ULiBJTiBOTyBFVkVOVCBTSEFMTCBUSEUgQVVUSE9SUyBPUiBDT1BZUklHSFQgSE9MREVSUyBCRQ0KTElBQkxFIEZPUiBBTlkgQ0xBSU0sIERBTUFHRVMgT1IgT1RIRVIgTElBQklMSVRZLCBXSEVUSEVSIElOIEFOIEFDVElPTg0KT0YgQ09OVFJBQ1QsIFRPUlQgT1IgT1RIRVJXSVNFLCBBUklTSU5HIEZST00sIE9VVCBPRiBPUiBJTiBDT05ORUNUSU9ODQpXSVRIIFRIRSBTT0ZUV0FSRSBPUiBUSEUgVVNFIE9SIE9USEVSIERFQUxJTkdTIElOIFRIRSBTT0ZUV0FSRS4NCg0KPT09PQ0KDQpBbGwgZmlsZXMgbG9jYXRlZCBpbiB0aGUgbm9kZV9tb2R1bGVzIGFuZCBleHRlcm5hbCBkaXJlY3RvcmllcyBhcmUNCmV4dGVybmFsbHkgbWFpbnRhaW5lZCBsaWJyYXJpZXMgdXNlZCBieSB0aGlzIHNvZnR3YXJlIHdoaWNoIGhhdmUgdGhlaXINCm93biBsaWNlbnNlczsgd2UgcmVjb21tZW5kIHlvdSByZWFkIHRoZW0sIGFzIHRoZWlyIHRlcm1zIG1heSBkaWZmZXIgZnJvbQ0KdGhlIHRlcm1zIGFib3ZlLg0K";
            var bytes = Convert.FromBase64String(s);
            return File(bytes, "text/plain");
        }
    }
}