using Ensco.Irma.Data.Repositories.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Services.Job
{
    public class JobService
    {
        private readonly vw_JobRepository jobRepository;

        public JobService(vw_JobRepository jobRepository)
        {
            this.jobRepository = jobRepository;
        }

        public IEnumerable<vw_Job> GetByStatus(string status)
        {
            return jobRepository.Filter(j => j.Status.Contains(status)).ToList();
        }

        public IEnumerable<vw_Job> GetByStatus(string[] status)
        {
            return jobRepository.Filter(j => status.Contains(j.Status)).ToList();
        }
    }
}
