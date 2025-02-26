using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace common.Helpers
{
    public class CloneHelper
    {
        /// <summary>
        /// 로그 생성을 위해 초기 로드한 데이터를 _origin에 반영시 _model과 같은 메모리 영역사용으로 인해
        /// _model에 변경이 있을 경우 _origin에도 수정이 진행
        /// _model이 수정 되어도_origin에 영향을 받지 않기 위해 CloneHelper클래스 및 DeepCopy 생성
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T source) where T : new()
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var type = typeof(T);
            var clone = new T();

            // 속성 복사
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || !prop.CanWrite) continue;
                if (prop.GetIndexParameters().Length > 0) continue; // 인덱서 제외

                var value = prop.GetValue(source, null);
                prop.SetValue(clone, value, null);
            }

            // 필드 복사
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = field.GetValue(source);
                field.SetValue(clone, value);
            }

            return clone;
        }
    }
}
