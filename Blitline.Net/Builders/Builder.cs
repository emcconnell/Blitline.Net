﻿using System;
using System.Collections.Generic;
using Blitline.Net.Functions;
using Blitline.Net.Functions.Builders;
using Blitline.Net.Request;

namespace Blitline.Net.Builders
{
    public abstract class Builder<T> : UberBuilder<T> where T : Function
    {
        protected List<BlitlineFunction> Functions { get; set; }

        protected Builder()
        {
            Functions = new List<BlitlineFunction>();
        }
        
        public override T Build()
        {
            var o = BuildImp();
            o.Functions.AddRange(Functions);
            return o;
        }

        public Builder<T> WithAnnotateFunction(Func<AnnotateFunctionBuilder, AnnotateFunction> build)
        {
            Functions.Add(build(new AnnotateFunctionBuilder()));
            return this;
        }

        public Builder<T> WithAppendFunction(Func<AppendFunctionBuilder, AppendFunction> build)
        {
            Functions.Add(build(new AppendFunctionBuilder()));
            return this;
        }

        public Builder<T> WithBlurFunction(Func<BlurFunctionBuilder, BlurFunction> build)
        {
            Functions.Add(build(new BlurFunctionBuilder()));
            return this;
        }

        public Builder<T> WithCompositeFunction(Func<CompositeFunctionBuilder, CompositeFunction> build)
        {
            Functions.Add(build(new CompositeFunctionBuilder()));
            return this;
        }

        public Builder<T> WithContrastFunction(Func<ContrastFunctionBuilder, ContrastFunction> build)
        {
            Functions.Add(build(new ContrastFunctionBuilder()));
            return this;
        }

        public Builder<T> WithContrastStretchChannelFunction(Func<ContrastStretchChannelFunctionBuilder, ContrastStretchChannelFunction> build)
        {
            Functions.Add(build(new ContrastStretchChannelFunctionBuilder()));
            return this;
        }

        public Builder<T> WithCropFunction(Func<CropFunctionBuilder, CropFunction> build)
        {
            Functions.Add(build(new CropFunctionBuilder()));
            return this;
        }

        public Builder<T> WithDeskewFunction(Func<DeskewFunctionBuilder, DeskewFunction> build)
        {
            Functions.Add(build(new DeskewFunctionBuilder()));
            return this;
        }

        public Builder<T> WithEqualizeFunction(Func<EqualizeFunctionBuilder, EqualizeFunction> build)
        {
            Functions.Add(build(new EqualizeFunctionBuilder()));
            return this;
        }

        public Builder<T> WithGammaChannelFunction(Func<GammaChannelFunctionBuilder, GammaChannelFunction> build)
        {
            Functions.Add(build(new GammaChannelFunctionBuilder()));
            return this;
        }

    }
}