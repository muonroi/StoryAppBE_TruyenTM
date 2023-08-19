using AutoMapper;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Categories;
using MuonRoiSocialNetwork.Domains.Interfaces.Commands.Stories;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Category;
using MuonRoiSocialNetwork.Domains.Interfaces.Queries.Stories;
using Slugify;
using System.Globalization;
using System.Text;

namespace MuonRoiSocialNetwork.Application.Commands.Base.Stories
{
    /// <summary>
    /// Handler base story
    /// </summary>
    public class BaseStoriesCommandHandler
    {
        /// <summary>
        /// property _mapper
        /// </summary>
        protected readonly IMapper _mapper;
        /// <summary>
        /// property get config
        /// </summary>
        protected readonly IConfiguration _configuration;
        /// <summary>
        /// property _storiesQuerie
        /// </summary>
        protected readonly IStoriesQueries _storiesQuerie;
        /// <summary>
        /// property _storiesRepository
        /// </summary>
        protected readonly IStoriesRepository _storiesRepository;
        /// <summary>
        /// Create new slug
        /// </summary>
        protected readonly SlugHelper _slugHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        /// <param name="storiesQuerie"></param>
        /// <param name="storiesRepository"></param>
        protected BaseStoriesCommandHandler(IMapper mapper, IConfiguration configuration, IStoriesQueries storiesQuerie, IStoriesRepository storiesRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _storiesRepository = storiesRepository;
            _storiesQuerie = storiesQuerie;
            _slugHelper = new();
        }
        /// <summary>
        /// Nomarlize text after genarate text to slug
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected string NormalizeString(string input)
        {
            var normalizedStringBuilder = new StringBuilder();
            foreach (char c in input.Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    normalizedStringBuilder.Append(c);
                }
            }

            return normalizedStringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
